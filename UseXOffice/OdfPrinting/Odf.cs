using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections;
using System.Diagnostics;

using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;

// Class for manipulating files in Open Document Format
public abstract class Odf
{
	public static string OoExe;			// path to soffice.exe
	public static string SaveDir;		// default directory, where documents are saved before printing

	protected string templateFile;
	protected XmlDocument doc;
	protected string fileToSave;

	public Odf(string odfFile)
	{
		Load(odfFile);
	}

	#region OoExe

	static void SetOoExe(string path)
	{
		if(OoExe == null && File.Exists(path))
		{
			OoExe = path;
		}
	}

	static Odf()
	{
		SetOoExe(@"C:\Program Files\OpenOffice.org 2.3\program\soffice.exe");
		SetOoExe(@"C:\Program Files\OpenOffice.org 2.2\program\soffice.exe");
		SetOoExe(@"C:\Program Files\OpenOffice.org 2.0\program\soffice.exe");
		SetOoExe(@"C:\Program Files\OpenOffice.org 3.2\program\soffice.exe");
		SetOoExe(@"C:\Program Files\OpenOffice.org 3.1\program\soffice.exe");
		SetOoExe(@"C:\Program Files\OpenOffice.org 3.0\program\soffice.exe");
		SetOoExe(@"C:\Program Files\OpenOffice.org 2.6\program\soffice.exe");
		SetOoExe(@"C:\Program Files\OpenOffice.org 2.5\program\soffice.exe");
		SetOoExe(@"C:\Program Files\OpenOffice.org 2.4\program\soffice.exe");
        SetOoExe(@"C:\Program Files (x86)\LibreOffice 4\program\soffice.exe");

		SaveDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
	}

	#endregion

	// Return odf file extension
	// i.e. "odt" for open document text, "ods" for open document spreadsheet
	protected abstract string GetFileExt();

	// Return type of OpenOffice aplication.
	// This is used to build "-writer" or "-calc" parameters to soffice.exe
	protected abstract string GetOoAppType();

	public virtual void Load(string templateFile)
	{
		this.templateFile = templateFile;

		// Read content.xml
		using(ZipInputStream zis = new ZipInputStream(File.OpenRead(templateFile)))
		{
			ZipEntry ze;
			while((ze = zis.GetNextEntry()) != null)
			{
				if(ze.Name == "content.xml")
				{
					StreamReader sr = new StreamReader(zis, Encoding.UTF8);
					string text = sr.ReadToEnd();
					doc = new XmlDocument();
					doc.LoadXml(text);
					break;
				}
			}
		}
	}

	public void Save(string fileName)
	{
		int count;
		byte[] buf = new byte[4096];
		DateTime now = DateTime.Now;
		this.fileToSave = fileName;

		using(ZipInputStream zis = new ZipInputStream(File.OpenRead(templateFile)))
		{
			using(ZipOutputStream zos = new ZipOutputStream(new FileStream(fileName, FileMode.Create)))
			{
				ZipEntry ze;
				while((ze = zis.GetNextEntry()) != null)
				{
					ZipEntry entry = new ZipEntry(ze.Name);
					entry.DateTime = now;
					zos.PutNextEntry(entry);

					if(ze.Name == "content.xml")
					{
						string text = doc.OuterXml;
						byte[] textBytes = Encoding.UTF8.GetBytes(text);
						zos.Write(textBytes, 0, textBytes.Length);
					}
					else
					{
						while((count = zis.Read(buf, 0, buf.Length)) > 0)
						{
							zos.Write(buf, 0, count);
						}
					}
				}

				zos.Finish();
			}
		}
	}

	private Process RunOo(string args)
	{
		if(OoExe == null)
		{
			throw new Exception("OpenOffice not found - either not installed or at unknown path");
		}

		Process p = new Process();
		p.StartInfo.FileName = OoExe;
		p.StartInfo.WorkingDirectory = Path.GetDirectoryName(OoExe);
		p.StartInfo.Arguments = "-" + GetOoAppType() + " " + args;	// i.e. -writer "sample.odt"
		p.Start();
		return p;
	}

	private Process RunOo(string args, string fileName)
	{
		return RunOo(args + " \"" + fileName + "\"");
	}

	// Return filename to be used for save without filename param
	private string GetFileToSave()
	{
		string fileName;
		int no = 0;

		if(fileToSave != null)
		{
			return fileToSave;
		}

		// Find unique name in the SaveDir
		do
		{
			fileName = Path.Combine(SaveDir, String.Format("{0}_{1:yyyy_MM_dd_HH_mm}{2}.{3}",
				Path.GetFileNameWithoutExtension(templateFile),
				DateTime.Now,
				no == 0 ? "" : "(" + (no++) + ")",
				GetFileExt())
				);
		}
		while(File.Exists(fileName));
		return fileName;
	}

	public void Print(string fileToSave)
	{
		Save(fileToSave);
		RunOo("-p", fileToSave);
	}

	public void Print()
	{
		Print(GetFileToSave());
	}

	public void OpenInOo(string fileToSave)
	{
		Save(fileToSave);
		RunOo("", fileToSave);
	}

	public void OpenInOo()
	{
		OpenInOo(GetFileToSave());
	}
}
