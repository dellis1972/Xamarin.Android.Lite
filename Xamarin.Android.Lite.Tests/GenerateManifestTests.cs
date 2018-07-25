﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Android.Lite.Tasks;

namespace Xamarin.Android.Lite.Tests
{
	[TestFixture]
	public class GenerateManifestTests
	{
		MockBuildEngine engine;
		string temp;

		[SetUp]
		public void SetUp ()
		{
			temp = Path.GetTempFileName ();
			engine = new MockBuildEngine ();
		}

		[TearDown]
		public void TearDown ()
		{
			File.Delete (temp);
		}

		[Test]
		public void Generate ()
		{
			var task = new GenerateManifest {
				BuildEngine = engine,
				DestinationFile = temp,
				PackageName = "com.test.app",
				ApplicationClass = "Test.App",
				VersionCode = "12",
				VersionName = "2.0.0",
			};
			Assert.IsTrue (task.Execute (), "Execute failed!");

			using (var stream = File.OpenRead (temp)) {
				var manifest = AndroidManifest.Create (stream);
				var xml = @"<manifest xmlns:android=""http://schemas.android.com/apk/res/android"" android:versionCode=""12"" android:versionName=""2.0.0"" package=""com.test.app"" platformBuildVersionCode=""27"" platformBuildVersionName=""8.1.0"">
  <uses-sdk android:minSdkVersion=""19"" android:targetSdkVersion=""27"" />
  <uses-permission android:name=""android.permission.INTERNET"" />
  <uses-permission android:name=""android.permission.READ_EXTERNAL_STORAGE"" />
  <uses-permission android:name=""android.permission.WRITE_EXTERNAL_STORAGE"" />
  <application android:label=""Xamarin.Android.Lite"" android:icon=""2130903040"" android:name=""android.app.Application"" android:debuggable=""true"" android:allowBackup=""true"">
    <meta-data android:name=""Xamarin.Android.Lite.Application"" android:value=""Test.App"" />
    <activity android:theme=""2131493263"" android:label=""Xamarin.Android.Lite"" android:icon=""2130903040"" android:name=""md5bff8b7c7908ce4fe5d805acf2300a9b4.MainActivity"" android:configChanges=""1152"">
      <intent-filter>
        <action android:name=""android.intent.action.MAIN"" />
        <category android:name=""android.intent.category.LAUNCHER"" />
      </intent-filter>
    </activity>
    <provider android:name=""mono.MonoRuntimeProvider"" android:exported=""false"" android:authorities=""com.test.app.mono.MonoRuntimeProvider.__mono_init_"" android:initOrder=""2147483647"" />
    <receiver android:name=""mono.android.Seppuku"">
      <intent-filter>
        <action android:name=""mono.android.intent.action.SEPPUKU"" />
        <category android:name=""mono.android.intent.category.SEPPUKU.com.test.app"" />
      </intent-filter>
    </receiver>
  </application>
</manifest>";
				Assert.AreEqual (xml, manifest.Document.ToString ());
			}
		}
	}
}
