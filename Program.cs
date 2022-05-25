using EncryptionDecryptionUsingSymmetricKey;
using System;
using TestConsoleApplication;

namespace rsaproject
{
	class Program
	{
		static void Main(string[] args)
		{
			int p = pbul();
			int q = qbul();
			int n = p * q;
			int phi = (p - 1) * (q - 1);

			int e = 0;

			int d = 0;

			while (true)
			{



				Random random = new Random();

				int randsayi = random.Next(1, phi);
				if (OBEB(randsayi, phi) == 1)
				{
					e = randsayi;
					break;

				}
			}
			d = oklid(e, phi);

			Console.WriteLine("n: " + n + ", " + "e: " + e + ", " + "d: " + d);
			Console.WriteLine("genel anahtar ( n,e ) :(" + n + ", " + e + ")");
			Console.WriteLine("ozel anahtar d : (" + d + ")");

			Console.WriteLine("Anahtar metnini giriniz: ");
			String text = Console.ReadLine();


			text = RSA(text, n, e);
			Console.WriteLine("sifreli metin : " + text);

			String desfparca = Desifre(text, d, n);
			Console.WriteLine("desifreli metin : " + desfparca);
		

			var key = desfparca;

			Console.WriteLine("Sifrelenecek metini  giriniz:  ");
			var str = Console.ReadLine();
			var encryptedString = AesOperation.EncryptString(key, str);
			Console.WriteLine($"Sifreli Metin = {encryptedString}");

			var decryptedString = AesOperation.DecryptString(key, encryptedString);
			Console.WriteLine($"Desifreli metin  = {decryptedString}");

			digitalSign.dgsign(decryptedString);
			Console.ReadKey();


		}
		static int oklid(int d, int f)
		{  //d=e f=phi gelicek


			int x1, x2, x3, y1, y2, y3;
			x1 = 1; x2 = 0; x3 = f; //p
			y1 = 0; y2 = 1; y3 = d; //d



			int q = 0, i = 1;
			int t1 = 0, t2 = 0, t3 = 0;
			do
			{
				if (i == 1)
				{
					q = x3 / y3;
					t1 = x1 - (q * y1);
					t2 = x2 - (q * y2);
					t3 = x3 - (q * y3);
				}
				else
				{
					x1 = y1; x2 = y2; x3 = y3;
					y1 = t1; y2 = t2; y3 = t3;
					q = x3 / y3;
					t1 = x1 - (q * y1);
					t2 = x2 - (q * y2);
					t3 = x3 - (q * y3);
				}
				i++;

				if (y3 == 0)
				{
					break;
				}

			} while (y3 != 1);

			if (y3 == 0)
			{
				Console.WriteLine("Sayinin tersi yoktur!!!!");
			}
			else
			{
				//Console.WriteLine("nSayinin tersi: " + y2);
			}

			return y2 + f;
		}

		private static Boolean asalSayi(int n)
		{
			int i;
			for (i = 2; i <= Math.Sqrt(n); i++)
			{
				if (n % i == 0)
				{
					return false;
				}
			}
			return true;
		}

		public static int pbul()
		{

			int p;
			while (true)
			{


				Random random1 = new Random();
				p = random1.Next(100, 9999);

				if (asalSayi(p))
				{
					Console.WriteLine(" p degeri :" + p);
					break;

				}

			}
			return p;
		}

		public static int qbul()
		{
			int q;
			while (true)
			{

				Random random2 = new Random();
				q = random2.Next(100, 9999);

				if (asalSayi(q))
				{
					Console.WriteLine(" q degeri :" + q);

					break;

				}

			}

			return q;
		}

		static int OBEB(int x, int y)
		{


			int min = Math.Min(x, y);
			int obeb = 1;
			for (int i = 2; i <= min; i++)
			{
				if (x % i == 0 && y % i == 0)
				{
					obeb = i;

				}

			}
			return obeb;


		}

		static double ustelMod(int a, int e, int n)
		{ //a^e(mod(n))

			double _a = a % n;
			double _e = e;
			if (e == 0) return 1;
			while (_e > 1)
			{
				_a *= a;
				_a %= n;
				_e--;

			}

			return _a;

		}


		static String RSA(String metin, int n, int e) //m^e(mod(n)) 

		{
			String strn = Convert.ToString(n);  //int -->string
			int lcipher = strn.Length;
			int lclear = strn.Length - 1;


			String birlesmisHal = "";
			char[] chars = metin.ToCharArray(); //stringi diziye at

			for (int i = 0; i < chars.Length; i++)
			{
				Console.WriteLine(chars[i]); //dizinin elmanlarını yazar
				int ascii = (int)chars[i];
				Console.WriteLine("ascii degeri: " + ascii);
				String sascii = Convert.ToString(ascii);  //asci -> stri

				while (sascii.Length < 3)
				{
					sascii = 0 + sascii;
				}

				Console.WriteLine("sola 0 eklenmis hali: " + sascii);
				birlesmisHal += sascii;


			}

			Console.WriteLine("en son sola sıfır ekli hali: " + birlesmisHal);


			while (birlesmisHal.Length % lclear != 0)
			{
				birlesmisHal = birlesmisHal + 0;
			}
			Console.WriteLine("lclear bolunmus hali saga 0 ekli tamam hali: " + birlesmisHal);


			int gond;
			String enSonHal = "";

			for (int i = 0; i < birlesmisHal.Length; i += lclear)
			{
				
				String parca = birlesmisHal.Substring(i, lclear);
				Console.WriteLine("parca: " + parca);

				gond = Convert.ToInt32(parca);     //str-->int
				double gelendeger = ustelMod(gond, e, n);
				Console.WriteLine("modgelen deger:  " + gelendeger);
				String strn2 = Convert.ToString(gelendeger);  //long -->string
				while (strn2.Length < lcipher)
				{
					strn2 = 0 + strn2;
				}
				enSonHal += strn2;
			}

			return enSonHal;
		}


		static String Desifre(String enSonHal, int d, int n) //m^d mod(n)
		{

			//String ustelmodgelen = "";
			String strn = Convert.ToString(n);  //int -->string
			int lcipher = strn.Length;
			int lclear = strn.Length - 1;
			String enSonHal2 = "";
			Console.WriteLine("--------desifre-----");
			for (int i = 0; i < enSonHal.Length; i += lcipher)
			{

				String parca2 = enSonHal.Substring(i, lcipher);

				Console.WriteLine("parca2: " + parca2);

				int gond2 = Convert.ToInt32(parca2);     //str-->int
				double dustelmod = ustelMod(gond2, d, n);
				Console.WriteLine("desifreden ustel gelen deger: " + dustelmod);
				String strn2 = Convert.ToString(dustelmod);

				while (strn2.Length < lclear)
				{
					strn2 = 0 + strn2;
				}
				Console.WriteLine("sola ekli hali desifre: " + strn2);
				enSonHal2 += strn2;

			}
			Console.WriteLine("sola 0 ekli tamam hali desifreli anahtar: " + enSonHal2);

			while (enSonHal2.Length % 3 != 0)
			{

				enSonHal2 = enSonHal2.Substring(0, enSonHal2.Length - 1); //amac ascııye tamamlamak 3 un katı olsun dıye 

			}
			String desfbirlesmis = "";
			char karakter;
			for (int i = 0; i < enSonHal2.Length; i += 3)
			{

				String desfparca = enSonHal2.Substring(i, 3);
				Console.WriteLine("parcalihali 3lu:" + desfparca);
				int desfascii = Convert.ToInt32(desfparca);  //string-->ascıı(inT)
				karakter = (char)desfascii;
				Console.WriteLine("Ascii karsiligi: " + karakter);
				desfbirlesmis += karakter;
			}


			return desfbirlesmis;

		}

	}
}