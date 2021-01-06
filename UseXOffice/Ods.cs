using System;
using System.Xml;
using System.Collections;

// Class for printing of odt files in OpenOffice
public class Ods : Odf
{
	private OdsTableCollection tables;
	private Hashtable cellByValue;

	public Ods(string odsFile) : base(odsFile)
	{
		InitGetCellByValue();
	}

	#region Collections

	public class OdsCell
	{
		private OdsRow row;
		private XmlNode node;
		private XmlAttribute valueTypeAttr;

		public OdsCell(OdsRow row, XmlNode node)
		{
			this.row = row;
			this.node = node;
			valueTypeAttr = node.Attributes["office:value-type"];
		}

		// Value type - i.e. string, float or date
		public string ValueType
		{
			get
			{
				if(valueTypeAttr != null)
				{
					return valueTypeAttr.Value;
				}
				return null;
			}
			set
			{
				if(valueTypeAttr == null)
				{
					valueTypeAttr = node.OwnerDocument.CreateAttribute("office:value-type");
					node.Attributes.Append(valueTypeAttr);
				}
				valueTypeAttr.Value = value;
			}
		}

		// Retrieve or set attribute value
		private string HandleValueAttr(string attrName, string value, bool retrieve)
		{
			XmlAttribute attr = node.Attributes[attrName];
			if(retrieve)
			{
				if(attr != null)
				{
					return attr.Value;
				}
				return null;
			}
			if(attr == null)
			{
				attr = node.OwnerDocument.CreateAttribute(attrName);
				node.Attributes.Append(attr);
			}
			return attr.Value = value;
		}

		// Retrieve or set node inner text
		private string HandleValueNode(string nodeName, string value, bool retrieve)
		{
			foreach(XmlNode xn in node.ChildNodes)
			{
				if(xn.Name == nodeName)
				{
					if(!retrieve)
					{
						xn.InnerText = value;
					}
					return xn.InnerText;
				}
			}
			if(retrieve)
			{
				return null;
			}
			XmlNode valueNode = node.OwnerDocument.CreateElement(nodeName);
			node.AppendChild(valueNode);
			return valueNode.InnerText = value;
		}

		// Retrieve or set cell value
		private string HandleValue(string value, bool retrieve)
		{
			switch(ValueType)
			{
				case "percentage":
				case "currency":
				case "float":
				{
					return HandleValueAttr("office:value", value, retrieve);
				}
				case "date":
				{
					return HandleValueAttr("office:date-value", value, retrieve);
				}
				case "time":
				{
					return HandleValueAttr("office:time-value", value, retrieve);
				}
				case "boolean":
				{
					return HandleValueAttr("office:boolean-value", value, retrieve);
				}
				default:		// "string"
				{
					return HandleValueNode("text:p", value, retrieve);
				}
			}
		}

		public string Value
		{
			get
			{
				return HandleValue(null, true);
			}
			set
			{
				HandleValue(value, false);
			}
		}

		public int Row
		{
			get
			{
				return row.Table.Rows.IndexOf(row);
			}
		}

		public int Column
		{
			get
			{
				return row.Cells.IndexOf(this);
			}
		}
	}

	public class OdsCellCollection : IEnumerable
	{
		private OdsRow row;
		private OdsCell[] cells;

		public OdsCellCollection(OdsRow row)
		{
			this.row = row;
			BuildCells();
		}

		private void BuildCells()
		{
			ArrayList list = new ArrayList();
			XmlNodeList nodes = row.XmlNode.ChildNodes;
			for(int i = 0; i < nodes.Count; i++)
			{
				XmlNode node = nodes[i];
				if(node.Name == "table:table-cell")
				{
					// Handle repeating nodes
					XmlAttribute repeatAttr = node.Attributes["table:number-columns-repeated"];
					if(repeatAttr != null)
					{
						int repeatCount = int.Parse(repeatAttr.Value);
						node.Attributes.Remove(repeatAttr);
						while(--repeatCount > 0)
						{
							XmlNode cloned = node.Clone();
							row.XmlNode.InsertAfter(cloned, node);
						}
					}
					list.Add(new OdsCell(row, node));
				}
			}
			cells = (OdsCell[]) list.ToArray(typeof(OdsCell));
		}

		public int Count
		{
			get
			{
				return cells.Length;
			}
		}

		public OdsCell this[int index]
		{
			get
			{
				return cells[index];
			}
		}

		public int IndexOf(OdsCell value)
		{
			return Array.IndexOf(cells, value);
		}

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return cells.GetEnumerator();
		}

		#endregion
	}

	public class OdsRow
	{
		private OdsTable table;
		private XmlNode node;
		private OdsCellCollection cells;

		public OdsRow(OdsTable table, XmlNode node)
		{
			this.table = table;
			this.node = node;
			this.cells = new OdsCellCollection(this);
		}

		public OdsCellCollection Cells
		{
			get
			{
				return cells;
			}
		}

		public OdsTable Table
		{
			get
			{
				return table;	
			}
		}

		public XmlNode XmlNode
		{
			get
			{
				return node;
			}
		}

		// Insert copy of this row after this row.
		public void CopyDown()
		{
			node.ParentNode.AppendChild(node.Clone());
			table.RebuildRows();
		}
	}

	public class OdsRowCollection : IEnumerable
	{
		private OdsTable table;
		private OdsRow[] rows;

		public OdsRowCollection(OdsTable table)
		{
			this.table = table;
			BuildRows();
		}

		private void BuildRows()
		{
			ArrayList list = new ArrayList();
			foreach(XmlNode node in table.XmlNode.ChildNodes)
			{
				if(node.Name == "table:table-row")
				{
					list.Add(new OdsRow(table, node));
				}
			}
			rows = (OdsRow[]) list.ToArray(typeof(OdsRow));
		}
		
		public int Count
		{
			get
			{
				return rows.Length;
			}
		}

		public OdsRow this[int index]
		{
			get
			{
				return rows[index];
			}
		}

		public int IndexOf(OdsRow value)
		{
			return Array.IndexOf(rows, value);
		}

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return rows.GetEnumerator();
		}

		#endregion
	}

	public class OdsTable
	{
		private XmlNode node;
		private Ods document;
		private OdsRowCollection rows;

		public OdsTable(Ods document, XmlNode node)
		{
			this.document = document;
			this.node = node;
			RebuildRows();
		}

		public string Name
		{
			get
			{
				return node.Attributes["table:name"].Value;
			}
		}

		public Ods Document
		{
			get
			{
				return document;
			}
		}

		public OdsRowCollection Rows
		{
			get
			{
				return rows;
			}
		}

		public OdsCell this[int row, int column]
		{
			get
			{
				return Rows[row].Cells[column];
			}
		}

		public OdsCell this[string name]
		{
			get
			{
				name = name.ToUpper();
				int col = 0;
				int i;
				for(i = 0; i < name.Length; i++)
				{
					int num = name[i] - 'A';
					if(num < 0 || num > 'Z' - 'A')
					{
						break;
					}
					num++;
					col = (col * ('Z' - 'A' + 1)) + num;
				}
				int row = 0;
				for(; i < name.Length; i++)
				{
					int num = name[i] - '0';
					if(num < 0 || num > '9' - '0')
					{
						break;
					}
					row = row * 10 + num;
				}
				return this[row - 1, col - 1];	
			}
		}

		public XmlNode XmlNode
		{
			get
			{
				return node;
			}
		}

		public void RebuildRows()
		{
			this.rows = new OdsRowCollection(this);
		}
	}

	// Collection used to access table sheets
	public class OdsTableCollection : IEnumerable
	{
		private Ods ods;
		private Hashtable ht;
		private string[] tableNames;

		public OdsTableCollection(Ods ods)
		{
			this.ods = ods;
			this.ht = new Hashtable();
			PopulateTables(ods.doc);
		}

		public int Count
		{
			get
			{
				return ht.Count;
			}
		}

		public OdsTable this[string tableName]
		{
			get
			{
				return (OdsTable) ht[tableName];
			}
		}

		public string[] TableNames
		{
			get
			{
				if(tableNames == null)
				{
					tableNames = new string[ht.Count];
					ht.Keys.CopyTo(tableNames, 0);
				}
				return tableNames;
			}
		}

		private void PopulateTables(XmlNode node)
		{
			if(node.Name == "table:table")
			{
				OdsTable table = new OdsTable(ods, node);
				ht[table.Name] = table;
			}
			else
			{
				foreach(XmlNode child in node.ChildNodes)
				{
					PopulateTables(child);
				}
			}
		}
		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{			
			return ht.Values.GetEnumerator();
		}

		#endregion
	}


	// Collection used to access document input fields
	public class OdtDocFields
	{
		private Odt odt;
		private Hashtable ht;
		private string[] fieldNames;

		public OdtDocFields(Odt odt)
		{
			this.odt = odt;
			this.ht = new Hashtable();
		}

		public int Count
		{
			get
			{
				return ht.Count;
			}
		}

		public string this[string fieldName]
		{
			get
			{
				return ((XmlNode)(ht[fieldName])).InnerText;
			}
			set
			{
				((XmlNode)(ht[fieldName])).InnerText = (string) value;
			}
		}

		public void AddNode(XmlNode node)
		{
			string desc = node.Attributes["text:description"].Value;
			ht[desc] = node;
		}

		public string[] FieldNames
		{
			get
			{
				if(fieldNames == null)
				{
					fieldNames = new string[ht.Count];
					ht.Keys.CopyTo(fieldNames, 0);
				}
				return fieldNames;
			}
		}
	}

	#endregion

	public OdsTableCollection Tables
	{
		get
		{
			return tables;
		}
	}

	private void InitGetCellByValue()
	{
		cellByValue = new Hashtable();
		foreach(OdsTable table in Tables)
		{
			foreach(OdsRow row in table.Rows)
			{
				foreach(OdsCell cell in row.Cells)
				{
					string cellValue = cell.Value;
					if(cellValue != null)
					{
						cellByValue[cellValue] = cell;
					}
				}
			}
		}
	}

	// Return cell by it's value
	public Ods.OdsCell GetCellByValue(string text)
	{
		return (OdsCell) cellByValue[text];
	}

	protected override string GetFileExt()
	{
		return "ods";
	}

	protected override string GetOoAppType()
	{
		return "calc";
	}

	public override void Load(string odsTemplateFile)
	{
		base.Load(odsTemplateFile);
		tables = new OdsTableCollection(this);
	}
}
