using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Android.App;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("GlucoMan_Mobile.Android")]
[assembly: AssemblyDescription("GlucoMan")]
[assembly: AssemblyConfiguration("Android version")]
[assembly: AssemblyCompany("-")]
[assembly: AssemblyProduct("GlucoMan_Mobile.Android")]
[assembly: AssemblyCopyright("Copyleft by Ing.Gabriele Monti - Forlì - Italia - 2021")]
[assembly: AssemblyTrademark("GlucoMan")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
[assembly: AssemblyVersion("0.4.0.0")]
[assembly: AssemblyFileVersion("0.4.0.0")]

// Add some common permissions, these can be removed if not needed
//[assembly: UsesPermission(Android.Manifest.Permission.Internet)]

// for File Picker API
[assembly: UsesPermission(Android.Manifest.Permission.WriteExternalStorage)]
