using System;
using System.Windows.Media;
using System.IO;
using System.Reflection;
using System.Linq;
using csutils.Test.Data;
using System.Text;
using NUnit.Framework;

namespace csutils.Test.Extensions
{
    [TestFixture]
    public class StringExtensionsTest
    {
#if CLR
        [TestCase]
        public void TestToColor()
        {
            Assert.AreEqual(Color.FromRgb(128, 128, 128), "808080".ToColor().Value);
            Assert.AreEqual(Color.FromRgb(128, 128, 128), "#808080".ToColor().Value);
            Assert.AreEqual(Color.FromRgb(255, 0, 0), "f00".ToColor().Value); 
            Assert.AreEqual(Color.FromRgb(255, 0, 0), "#f00".ToColor().Value);
            Assert.AreEqual(Color.FromRgb(255, 0, 0), "ff0000".ToColor().Value);
            Assert.AreEqual(Color.FromRgb(255, 0, 0), "#ff0000".ToColor().Value);
            Assert.IsNull("#ff00000".ToColor());
            Assert.IsNull("".ToColor());
            Assert.IsNull("test".ToColor());
        }
#endif

        [TestCase]
        public void TestToBool()
        {
            Assert.IsTrue("true".ToBool().Value);
            Assert.IsTrue("TRue".ToBool().Value);
            Assert.IsTrue("true ".ToBool().Value);
            Assert.IsFalse("false".ToBool().Value);
            Assert.IsFalse("FALse".ToBool().Value);
            Assert.IsFalse("false ".ToBool().Value);
            Assert.IsNull("falsé".ToBool());

            Assert.IsFalse("falsé".ToBool(false));
            Assert.IsTrue("falsé".ToBool(true));

        }

        [TestCase]
        public void TestToInt()
        {
            Assert.AreEqual(1, "1".ToInt().Value);
            Assert.AreEqual(1, " 1 ".ToInt().Value);
            Assert.IsNull("1p".ToInt());

            Assert.AreEqual(1, "1".ToInt(5));
            Assert.AreEqual(5, "1p".ToInt(5));
        }

        [TestCase]
        public void TestToDouble()
        {
            Assert.AreEqual(1, "1".ToDouble().Value);
            Assert.AreEqual(1.1, "1,1".ToDouble().Value);
            Assert.AreEqual(1, " 1 ".ToDouble().Value);
            Assert.IsNull("1p".ToDouble());

            Assert.AreEqual(1, "1".ToDouble(5));
            Assert.AreEqual(5, "1p".ToDouble(5));
        }

        [TestCase]
        public void TestTryAdd()
        {
            Assert.AreEqual("2", "1".TryAdd(1));
            Assert.AreEqual("2,1", "1".TryAdd(1.1));
            Assert.AreEqual("2,1px", "1px".TryAdd(1.1));
            Assert.AreEqual("x1px", "x1px".TryAdd(1.1));
        }

        [TestCase]
        public void TestIndexOf()
        {
            string[] strings = new string[] { "1", "2" };
            Assert.AreEqual(-1, strings.IndexOf("3"));
            Assert.AreEqual(1, strings.IndexOf("2"));
            Assert.AreEqual(0, strings.IndexOf("1"));
        }

        [TestCase]
        public void TestFillReplace()
        {
            Assert.AreEqual("Test", "Text".FillReplace("x", 's'));
            Assert.AreEqual("Text", "Text".FillReplace("s", 'x'));
            Assert.AreEqual("H         d", "Hello World".FillReplace("e.*l", ' '));
        }

        [TestCase]
        public void TestTrimQuotes()
        {
            Assert.AreEqual("Test", "\"Test".TrimQuotes());
            Assert.AreEqual("Test", "\"Test\"".TrimQuotes());
            Assert.AreEqual("Test", "'Test''".TrimQuotes());
            Assert.AreEqual("Test", "\"\"\"Test\"\"\"".TrimQuotes());
            Assert.AreEqual("", "".TrimQuotes());
        }

        [TestCase]
        public void TestRemoveExtension()
        {
            Assert.AreEqual("Test", "Test".RemoveExtension());
            Assert.AreEqual("Test", "Test.ext".RemoveExtension());
            Assert.AreEqual("Test", "Test.e".RemoveExtension());
            Assert.AreEqual("Test.", "Test..ext".RemoveExtension());           
        }

        [TestCase]
        public void TestIndexOfAny()
        {
            string[] strings = new string[] { "1", "2", "3", null, "10", null };
            Assert.AreEqual(-1, "456".IndexOfAny(strings));
            Assert.AreEqual(0, "123".IndexOfAny(strings));
            Assert.AreEqual(1, "012".IndexOfAny(strings));
            Assert.AreEqual(-1, "".IndexOfAny(strings));
        }

        [TestCase]
        public void TestEndsWith()
        {
            string[] strings = new string[] { "1", "2", "3", null, "10", null };
            Assert.IsFalse("Hello World".EndsWith(strings));
            Assert.IsTrue("Hello World2".EndsWith(strings));            
        }

        [TestCase]
        public void TestFileName()
        {
            Assert.AreEqual(@"test.", new FileInfo(@"test..ext").FileName());
            Assert.AreEqual(@"test", new FileInfo(@"test.ext").FileName());
            Assert.AreEqual(@"t", new FileInfo(@"t").FileName());
        }

        [TestCase]
        public void TestPath()
        {
            Assert.AreEqual(@"test\", @"test".PathWithSlash());
            Assert.AreEqual(@"test\", @"test\".PathWithSlash());
            Assert.AreEqual(@"test", @"test\".PathWithoutSlash());
            Assert.AreEqual(@"test", @"test".PathWithoutSlash());
        }

        [TestCase]
        public void TestChangeName()
        {
            Assert.AreEqual(new DirectoryInfo("C:\\test").FullName, new DirectoryInfo("C:\\temp").ChangeName("test").FullName);
            Assert.AreEqual(new DirectoryInfo("C:\\test").FullName, new DirectoryInfo("C:\\temp\\").ChangeName("test").FullName);
            Assert.AreEqual(new DirectoryInfo("C:\\temp").FullName, new DirectoryInfo("C:\\temp").ChangeName(null).FullName);
            Assert.AreEqual(new DirectoryInfo("C:\\").FullName, new DirectoryInfo("C:\\").ChangeName("test").FullName);
        }

        [TestCase]
        public void TestStartsWithNumeric()
        {
            Assert.IsFalse("Test".StartsWithNumeric());
            Assert.IsFalse("".StartsWithNumeric());
            Assert.IsTrue("1".StartsWithNumeric());
            Assert.IsTrue("-1".StartsWithNumeric());
        }

        [TestCase]
        public void TestToPath()
        {
            Assert.AreEqual(new FileInfo(@"C:\file.ext").FullName, @"C:\file.ext".ToPath().FullName); 
            Assert.AreEqual(new FileInfo(Directory.GetCurrentDirectory()+@"\file.ext").FullName, @"file.ext".ToPath().FullName);
            Assert.AreEqual(new FileInfo(Directory.GetCurrentDirectory() + @"\.htaccess").FullName, @".htaccess".ToPath().FullName);
            Assert.AreEqual(new DirectoryInfo(Directory.GetCurrentDirectory() + @"\dir").FullName, @"dir".ToPath().FullName);
            Assert.AreEqual( new DirectoryInfo(@"C:\dir").FullName, @"C:\dir".ToPath().FullName);

            Assert.IsNull(@"C:\file.ext".ToPath(true));
            Assert.IsNotNull(@"C:\Windows\notepad.exe".ToPath(true));
            Assert.IsNotNull(@"C:\".ToPath(true));
            Assert.IsNull(@"C:\f".ToPath(true));

            Assert.AreEqual(new FileInfo(@"C:\Windows\notepad.exe").FullName, @"%SystemRoot%\notepad.exe".ToPath().FullName);

            try
            {
                @"C:\file?.ext".ToPath(); //invalid character for filenames
                Assert.Fail();
            }
            catch
            {
                
            }
        }

        [TestCase]
        public void TestToFileInfo()
        {
            Assert.AreEqual(new FileInfo(@"C:\file.ext").FullName, @"C:\file.ext".ToFileInfo().FullName);
            Assert.AreEqual(new FileInfo(Directory.GetCurrentDirectory() + @"\file.ext").FullName, @"file.ext".ToFileInfo().FullName);
            Assert.AreEqual(new FileInfo(Directory.GetCurrentDirectory() + @"\.htaccess").FullName, @".htaccess".ToFileInfo().FullName);
            Assert.AreEqual(new FileInfo(Directory.GetCurrentDirectory() + @"\dir").FullName, @"dir".ToFileInfo().FullName);
            
            Assert.IsNull(@"C:\file.ext".ToPath(true));
            Assert.IsNotNull(@"C:\Windows\notepad.exe".ToPath(true));
            Assert.IsNotNull(@"C:\".ToPath(true));
            Assert.IsNull(@"C:\f".ToPath(true));

            Assert.AreEqual(new FileInfo(@"C:\Windows\notepad.exe").FullName, @"%SystemRoot%\notepad.exe".ToPath().FullName);

            try
            {
                @"C:\file?.ext".ToPath(); //invalid character for filenames
                Assert.Fail();
            }
            catch
            {

            }
        }

        [TestCase]
        public void TestToDirectoryInfo()
        {
            Assert.AreEqual(new DirectoryInfo(@"C:\file.ext").FullName, @"C:\file.ext".ToDirectoryInfo().FullName);
            Assert.AreEqual(new DirectoryInfo(Directory.GetCurrentDirectory() + @"\file.ext").FullName, @"file.ext".ToDirectoryInfo().FullName);
            Assert.AreEqual(new DirectoryInfo(Directory.GetCurrentDirectory() + @"\.htaccess").FullName, @".htaccess".ToDirectoryInfo().FullName);
            Assert.AreEqual(new DirectoryInfo(Directory.GetCurrentDirectory() + @"\dir").FullName, @"dir".ToDirectoryInfo().FullName);
            Assert.AreEqual(new DirectoryInfo(@"C:\dir").FullName, @"C:\dir".ToDirectoryInfo().FullName);

            Assert.IsNull(@"C:\file.ext".ToDirectoryInfo(true));
            Assert.IsNull(@"C:\Windows\notepad.exe".ToDirectoryInfo(true));
            Assert.IsNotNull(@"C:\".ToDirectoryInfo(true));
            Assert.IsNull(@"C:\f".ToDirectoryInfo(true));

            Assert.AreEqual(new DirectoryInfo(@"C:\Windows\notepad.exe").FullName, @"%SystemRoot%\notepad.exe".ToDirectoryInfo().FullName);

            try
            {
                @"C:\file?".ToPath(); //invalid character for filenames
                Assert.Fail();
            }
            catch
            {

            }
        }

        [TestCase]
        public void TestRemove()
        {
            Assert.AreEqual("Test!","Test123!".Remove(new System.Text.RegularExpressions.Regex("[0-9]+")));
            Assert.AreEqual("Test!!!?", "Test123?".Remove(new System.Text.RegularExpressions.Regex("[0-9]+"),'!'));

            Assert.AreEqual("Tes1", "Test123!".Remove("2!34t6"));
            Assert.AreEqual("    12 ?", "Test123?".Remove("Test3",' '));

            Assert.AreEqual("C:\\test.txt", "C:\\test.txt".Remove(CharacterType.InvalidPathCharacters));
            Assert.AreEqual("C:\\tes.txt", "C:\\tes?.txt".Remove(CharacterType.InvalidPathCharacters));

            Assert.AreEqual("https://user:pass@www.server.net:8080/res/./../page.php?p=value&p2=value", "https://user:pass@www.server.net:8080/res/./../page.php?p=value&p2=value".Remove(CharacterType.InvalidURLCharacters));
            Assert.AreEqual("https://user:pass@www.server.net:8080/res/./../page.php?p=value&p2=value", "https://user:pass@www.server.net:8080/res/./../page.php?p=value&p2<=value".Remove(CharacterType.InvalidURLCharacters));


            string str = RegexesTest.GenerateUnicodeString(32,255);
            Assert.AreEqual(str.Length, str.Remove(CharacterType.NonASCIICharacters).Length);
            Assert.AreEqual(str, str.Remove(CharacterType.NonASCIICharacters));
            Assert.AreEqual(str, (str+"Ā").Remove(CharacterType.NonASCIICharacters));
            Assert.AreEqual("", "䢷䢸䢹䢺䢻䢼䢽䢾䢿䣀䣁䣂䣃䣄䣅䣆䣇䣈䣉䣊䣋䣌䣍䣎䣏䣐䣑䣒䣓䣔䣕䣖䣗䣘䣙䣚䣛䣜䣝䣞䣟䣠䣡䣢䣣䣤䣥䣦䣧䣨䣩䣪䣫䣬䣭䣮䣯䣰䣱䣲䣳䣴䣵䣶䣷䣸䣹䣺䣻䣼䣽䣾䣿䤀䤁䤂䤃䤄䤅䤆䤇䤈䤉䤊䤋䤌䤍䤎䤏䤐䤑䤒䤓䤔䤕䤖䤗䤘䤙䤚䤛䤜䤝䤞䤟䤠䤡䤢䤣䤤䤥䤦䤧䤨䤩䤪䤫䤬䤭䤮䤯䤰䤱䤲䤳䤴䤵䤶䤷䤸䤹䤺䤻䤼䤽䤾䤿䥀䥁䥂䥃䥄䥅䥆䥇䥈䥉䥊䥋䥌䥍䥎䥏䥐䥑䥒䥓䥔䥕䥖䥗䥘䥙䥚䥛䥜䥝䥞䥟䥠䥡䥢䥣䥤䥥䥦䥧䥨䥩䥪䥫䥬䥭䥮䥯䥰䥱䥲䥳䥴䥵䥶䥷䥸䥹䥺䥻䥼䥽䥾䥿䦀䦁䦂䦃䦄䦅䦆䦇䦈䦉䦊䦋䦌䦍䦎䦏䦐䦑䦒䦓䦔䦕䦖䦗䦘䦙䦚䦛䦜䦝䦞䦟䦠䦡䦢䦣䦤䦥䦦䦧䦨䦩䦪䦫䦬䦭䦮䦯䦰䦱䦲䦳䦴䦵".Remove(CharacterType.NonASCIICharacters));
            Assert.AreEqual("!!!!!", "䣂䣃䣄䣅䢾".Remove(CharacterType.NonASCIICharacters, '!'));

            Assert.AreEqual("", "".Remove(CharacterType.Quotations));
            Assert.AreEqual("", "\"'“”‘’".Remove(CharacterType.Quotations));

            Assert.AreEqual("tt", " t t     ".Remove(CharacterType.Whitespaces));


            Assert.AreEqual("Test123!", "Te    st'12 3!".Remove(CharacterType.Quotations | CharacterType.Whitespaces));

        }

        [TestCase]
        public void TestToStream()
        {
            Assert.AreEqual(0, @"test".ToStream().Position);
            Assert.AreEqual(8, @"test".ToStream().Length); //2 bytes per character 
            Assert.AreEqual(4, @"t€st".ToStream(Encoding.ASCII).Length); //1 byte for every char
            Assert.AreEqual(@"t€st", @"t€st".ToStream(Encoding.Unicode).ToString(Encoding.Unicode));
        }


		[TestCase]
		public void TestFilter()
		{
			string[] data = new string[] { "abc","bcd","cde","def","\"test","H3llo W0rld","","Multi\nline" };

			var res = data.Filter("[def]");
			Assert.AreEqual(6, res.Length);
			Assert.AreEqual("bcd",res[0]);
			Assert.AreEqual("cde", res[1]);
			Assert.AreEqual("def", res[2]);
			Assert.AreEqual("\"test", res[3]);
			Assert.AreEqual("H3llo W0rld", res[4]);
			Assert.AreEqual("Multi\nline", res[5]);

			res = data.Filter("");
			Assert.AreEqual(8, res.Length);




			var res2 = data.Select(s => s).Filter("[def]").ToArray();
			Assert.AreEqual(6, res2.Length);
			Assert.AreEqual("bcd", res2[0]);
			Assert.AreEqual("cde", res2[1]);
			Assert.AreEqual("def", res2[2]);
			Assert.AreEqual("\"test", res2[3]);
			Assert.AreEqual("H3llo W0rld", res2[4]);
			Assert.AreEqual("Multi\nline", res2[5]);

		}

		[TestCase]
		public void TestIsAbsolutePath()
		{
			Assert.IsTrue("C:\\test.txt".IsAbsolutePath());
			Assert.IsTrue("C:\\".IsAbsolutePath());
			Assert.IsFalse("test.txt".IsAbsolutePath());

			Assert.IsFalse("google.com".IsAbsolutePath());
			Assert.IsTrue("http://google.com".IsAbsolutePath());
		}
    }
}
