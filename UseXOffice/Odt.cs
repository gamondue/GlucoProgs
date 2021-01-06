using System;
using System.Xml;
using System.Collections;

// Class for printing of odt files in OpenOffice
public class Odt : Odf
{
	OdtDocFields inputs;

	public Odt(string odtFile) : base(odtFile)
	{
	}

	#region Collections

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

	public OdtDocFields Inputs
	{
		get
		{
			return inputs;
		}
	}

	protected override string GetFileExt()
	{
		return "odt";
	}

	protected override string GetOoAppType()
	{
		return "writer";
	}

	private void PopulateInputs(XmlNode node)
	{
		if(node.Name == "text:text-input")
		{
			inputs.AddNode(node);
		}
		foreach(XmlNode child in node.ChildNodes)
		{
			PopulateInputs(child);
		}
	}

	public override void Load(string odtTemplateFile)
	{
		base.Load(odtTemplateFile);

		// Add all input fields in collection
		inputs = new OdtDocFields(this);
		PopulateInputs(doc.DocumentElement);
	}
}
