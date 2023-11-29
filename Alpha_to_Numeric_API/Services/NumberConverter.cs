namespace Alpha_to_Numeric_API.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class NumberConverter
    {
        // Birler, onlar ve ana kökleri ayırdığım anahtar ve değer çiftleri aşağıdaki gibidir. Section1
        private static KeyValuePair<string, int>[] birlerBirlik = new KeyValuePair<string, int>[]
        {
        new KeyValuePair<string, int>("sıfır", 0),
        new KeyValuePair<string, int>("bir", 1),
        new KeyValuePair<string, int>("iki", 2),
        new KeyValuePair<string, int>("üç", 3),
        new KeyValuePair<string, int>("dört", 4),
        new KeyValuePair<string, int>("beş", 5),
        new KeyValuePair<string, int>("altı", 6),
        new KeyValuePair<string, int>("yedi", 7),
        new KeyValuePair<string, int>("sekiz", 8),
        new KeyValuePair<string, int>("dokuz", 9),
        };

        private static KeyValuePair<string, int>[] onlarBirlik = new KeyValuePair<string, int>[]
        {
        new KeyValuePair<string, int>("on", 10),
        new KeyValuePair<string, int>("yirmi", 20),
        new KeyValuePair<string, int>("otuz", 30),
        new KeyValuePair<string, int>("kırk", 40),
        new KeyValuePair<string, int>("elli", 50),
        new KeyValuePair<string, int>("altmış", 60),
        new KeyValuePair<string, int>("yetmiş", 70),
        new KeyValuePair<string, int>("seksen", 80),
        new KeyValuePair<string, int>("doksan", 90)
        };

        private static KeyValuePair<string, int>[] buyukBirlik = new KeyValuePair<string, int>[]
        {
        new KeyValuePair<string, int>("yüz", 100),
        new KeyValuePair<string, int>("bin", 1000),
        new KeyValuePair<string, int>("milyon", 1000000),
        new KeyValuePair<string, int>("mily", 1000000)
        };
        // End of Section1


        // Anahtar değer çiftlerinin anahtar kısmının kodun ileriki aşamalarında kullanılması için oluşturduğum listeler. Section2

        private static List<string> tens = onlarBirlik.Select(pair => pair.Key).ToList();
        private static List<string> ones = birlerBirlik.Select(pair => pair.Key).ToList();
        private static List<string> bigs = buyukBirlik.Select(pair => pair.Key).ToList();
        private static List<string> combinedList = new List<string>(tens.Concat(ones).Concat(bigs));
        // End of Section2

        // Dışarıda kullanılmak üzere ana fonksiyonun kullanıldığı kısım. Section3
        public static string ConvertNumber(string text)
        {
            text = text.ToLower(); 
            string[] checkText = text.Split(" ");
            string outText = text;
            bool checkMain = false;
            // Input içerisinde çevirilecek sayı olup olmadığını kontrol eden kısım. Section3.1
            foreach(string check in checkText)
            {
                foreach(var nums in combinedList)
                {
                    if (check.Contains(nums))
                    {
                        checkMain = true;
                        break;
                    }
                    
                }
            } // End of Section 3.1




            while (checkMain)
            {
                string dividedText = text;
                bool isSpace = false;
                string lastText = "";
                // CombinedList dışındaki kellimelerin var olup olmadığının kontrol edildiği kısım. Section3.2
                foreach (var nums in combinedList)
                {
                    if (dividedText.IndexOf(nums) != -1)
                    {
                        dividedText = dividedText.Replace(nums, "");
                    }
                }

                foreach (char karakter in dividedText)
                {
                    if (karakter == ' ')
                    {
                        if (!isSpace)
                        {
                            lastText += karakter;
                            isSpace = true;
                        }
                    }
                    else
                    {
                        lastText += karakter;
                        isSpace = false;
                    }
                }
                if (lastText == "" || lastText == " ")
                {
                    int outL = AlphaToNumeric(text);
                    outText = RemoveSpaces(AddWords2g(text, outL));
                    checkMain = false;
                } 
                else
                {
                    // Bulunan sayıdan sonra varsa ilk kelimenin kontrol edildiği kısım. Section3.2.1
                    string firstWord = FindDifferenceAndNextWord(lastText,text );
                    foreach (var kelime in checkText)
                    {
                        if (kelime.Contains(firstWord))
                        {
                            string checkNumText = kelime;
                            foreach(var nums  in combinedList)
                            {
                                if (checkNumText.Contains(nums))
                                {
                                    firstWord = FindDifferenceAndNextWord(lastText, text);
                                    break;
                                }
                                else
                                {
                                    firstWord = checkNumText; 
                                    
                                }
                            }
                        }
                    } // End of Section3.2.1

                    // Bulunan ilk kelimeden sonra metnin sol ve sağ olarak ikiye bölündüğü kısım. Section3.2.2
                    string leftWords = DivideByWords(text, firstWord).Item2;
                    string rightWords = DivideByWords(text, firstWord).Item1;
                    


                    string right = "";

                    if (leftWords != "")
                    {
                        int outLeft = AlphaToNumeric(leftWords);
                        right = AddWords2g(leftWords, outLeft);
                    }
                    int outRight = AlphaToNumeric(rightWords);
                    string left = AddWords2g(rightWords, outRight);
                    outText = CheckOneMoreSpace(FinalWord(left, firstWord, right));
                    checkMain = false;
                    // End of Section3.2.2
                }

            } // End of Section3.2



            return outText;
        }
        // End of Section3


        // Verilen stringleri belirli kelimere göre ikiye bölen Tuple metodları. Section4
        private static Tuple<string, string> DivideByBin(string kelime)
        {
            int Index = kelime.IndexOf("bin");
            string sol = kelime.Substring(0, Index);
            string sag = kelime.Substring(Index + 3);

            return Tuple.Create(sol, sag);
        }

        private static Tuple<string, string> DivideByYuz(string kelime)
        {
            int Index = kelime.IndexOf("yüz");
            string sol = kelime.Substring(0, Index);
            string sag = kelime.Substring(Index + 3);

            return Tuple.Create(sol, sag);
        }

        private static Tuple<string, string> DivideByMilyon(string kelime)
        {
            int Index = kelime.IndexOf("milyon");
            string sol = kelime.Substring(0, Index);
            string sag = kelime.Substring(Index + 6);

            return Tuple.Create(sol, sag);
        }

        private static Tuple<string, string> DivideByWords(string kelime, string ayrac)
        {
            int Index = kelime.IndexOf(ayrac);
            string sol = kelime.Substring(0, Index);
            string sag = kelime.Substring(Index + ayrac.Length);

            return Tuple.Create(sol, sag);
        }
        // End of Section 4


        // Verilen stringler içerisindeki onluk kısımdan sonrasını çıkaran metod Section5
        private static int GivenAdd(string numb)
        {
            string givenTenStr = "";
            string givenOnesStr = "";
            int givenTenNum = 0;
            int givenOneNum = 0;

            foreach (var ten in tens)
            {
                int tenIndex = numb.IndexOf(ten);
                if (tenIndex != -1)
                {
                    string sag = numb.Substring(tenIndex + ten.Length);
                    givenTenStr = ten;
                    givenTenNum = Array.Find(onlarBirlik, x => x.Key == ten).Value;
                    foreach (var one in ones)
                    {
                        if (sag.IndexOf(one) != -1)
                        {
                            givenOnesStr = one;
                            givenOneNum = Array.Find(birlerBirlik, x => x.Key == ten).Value;
                        }
                    }
                }
                else
                {
                    foreach (var one in ones)
                    {
                        if (numb.IndexOf(one) != -1)
                        {
                            givenOnesStr = one;
                            givenOneNum = Array.Find(birlerBirlik, x => x.Key == one).Value;
                        }
                    }
                }
            }

            return givenTenNum + givenOneNum;
        }
        // End of Section5

        // Verilen stringler içerisindeki yüzlük kısmını çıkaran metod Section6
        private static int Yuzluk(string yuzluk)
        {
            int multNumYuz = 1;
            int addNumYuz = 0;
            int multLeft = 1;
            string yuzLeft = DivideByYuz(yuzluk).Item1;
            string yuzRight = DivideByYuz(yuzluk).Item2;

            if (GivenAdd(yuzLeft) == 0)
            {
                multLeft = 1;
            }
            else
            {
                multLeft = GivenAdd(yuzLeft);
            }

            multNumYuz = multNumYuz * multLeft;
            addNumYuz = addNumYuz + GivenAdd(yuzRight);

            return multNumYuz * 100 + addNumYuz;
        }
        // End of Section6

        // Girilen iki metin arasındaki farkın başladığı indeksi bulan fonksiyon Section7
        private static int FindDifferenceIndex(string text1, string text2)
        {
            int minLength = Math.Min(text1.Length, text2.Length);

            for (int i = 0; i < minLength; i++)
            {
                if (text1[i] != text2[i])
                {
                    return i;
                }
            }

            if (text1.Length != text2.Length)
            {
                return minLength;
            }

            return -1;
        }
        // End of Section7

        // Bulunan indexten itibaren iki stringi birleştiren metod Section8
        private static string InsertString(string original, int index, string textToInsert)
        {
            string textToInsert2 = textToInsert + " ";
            if (index < 0 || index > original.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Geçersiz indeks");
            }

            return original.Insert(index, textToInsert2);
        }
        // End of Section8


        // Sol ve Sağ olarak böldüğümüz ana metnin girdiği ana metod. Kısaca matematiksel işlemlerin yapıldığı metod. Section9
        private static int AlphaToNumeric(string text)
        {
            string wholeWord = text.Replace(" ", "");
            int binIndex = wholeWord.IndexOf("bin");
            int yuzIndex = wholeWord.IndexOf("yüz");
            int milyonIndex = wholeWord.IndexOf("milyon");
            int mainRoot = 0;
            int addNum = 0;
            int multNum = 1;

            if (milyonIndex != -1)
            {
                string milionLeft = DivideByMilyon(wholeWord).Item1;
                string milionRight = DivideByMilyon(wholeWord).Item2;
                mainRoot = 1000000;
                int rightMainRoot = 0;
                int multRightNum = 1;
                int addRightNum = 0;

                if (milionLeft.IndexOf("yüz") != -1)
                {
                    string childBinLeft = DivideByYuz(milionLeft).Item1;
                    string childBinRight = DivideByYuz(milionLeft).Item2;
                    multNum = 100;
                    multNum = multNum * (childBinLeft != "" ? GivenAdd(childBinLeft) : 1);
                    multNum = multNum + GivenAdd(childBinRight);
                }
                else if (milionLeft == "")
                {
                    multNum = 1;
                }
                else
                {
                    multNum = GivenAdd(milionLeft);
                }

                if (milionRight.IndexOf("bin") != -1)
                {
                    rightMainRoot = 1000;
                    string binRight = DivideByBin(milionRight).Item2;
                    string binLeft = DivideByBin(milionRight).Item1;

                    if (binLeft.IndexOf("yüz") != -1)
                    {
                        multRightNum = Yuzluk(binLeft);
                    }
                    else if (binLeft == "")
                    {
                        multRightNum = 1;
                    }
                    else
                    {
                        multRightNum = GivenAdd(binLeft);
                    }

                    if (binRight.IndexOf("yüz") != -1)
                    {
                        addRightNum = Yuzluk(binRight);
                    }
                    else
                    {
                        addRightNum = GivenAdd(binRight);
                    }
                }
                else if (milionRight.IndexOf("yüz") != -1)
                {
                    rightMainRoot = Yuzluk(milionRight);
                }
                else
                {
                    rightMainRoot = GivenAdd(milionRight);
                }

                addNum = multRightNum * rightMainRoot + addRightNum;
            }
            else if (binIndex != -1)
            {
                mainRoot = 1000;
                string binLeft = DivideByBin(wholeWord).Item1;
                string binRight = DivideByBin(wholeWord).Item2;

                if (binLeft.IndexOf("yüz") != -1)
                {
                    multNum = Yuzluk(binLeft);
                }
                else if (binLeft == "")
                {
                    multNum = 1;
                }
                else
                {
                    multNum = GivenAdd(binLeft);
                }

                if (binRight.IndexOf("yüz") != -1)
                {
                    addNum = Yuzluk(binRight);
                }
                else
                {
                    addNum = GivenAdd(binRight);
                }
            }
            else if (yuzIndex != -1)
            {
                mainRoot = Yuzluk(wholeWord);
            }
            else
            {
                addNum = GivenAdd(wholeWord);
            }

            return multNum * mainRoot + addNum;
        }
        // End of Section9

        // Indexi, artık metni ve işlemi yapılmış sayıyı ekleyen metod Section10
        private static string AddWords2g(string text, int output)
        {
            string finalText = text;
            bool boslukVar = false;
            string fixedText = "";

            foreach (var nums in combinedList)
            {
                if (finalText.IndexOf(nums) != -1)
                {
                    finalText = finalText.Replace(nums, "");
                }
            }

            foreach (char karakter in finalText)
            {
                if (karakter == ' ')
                {
                    if (!boslukVar)
                    {
                        fixedText += karakter;
                        boslukVar = true;
                    }
                }
                else
                {
                    fixedText += karakter;
                    boslukVar = false;
                }
            }
            string modifiedText = text;
            if (text != fixedText)
            {
                int startIndex = FindDifferenceIndex(text, fixedText);
                modifiedText = InsertString(fixedText, startIndex, output.ToString());
            }
            

            return modifiedText;
        }
        //End of Section10



        private static string FinalWord(string text1, string text2, string text3)
        {
            return text1 + text2 + text3;
        }
        private static string CheckOneMoreSpace(string cumle)
        {
            string[] kelimeler = cumle.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string duzeltilmisCumle = string.Join(" ", kelimeler);

            return duzeltilmisCumle;
        }

        // Verilen iki cümlenin arasındaki farkın ilk başladığı kelimeyi çıkaran metod Section11
        private static string FindDifferenceAndNextWord(string str1, string str2)
        {
            int farkIndeks = -1;
            for (int i = 0; i < Math.Min(str1.Length, str2.Length); i++)
            {
                if (str1[i] != str2[i])
                {
                    farkIndeks = i;
                    break;
                }
            }

            if (farkIndeks != -1 && farkIndeks < Math.Min(str1.Length, str2.Length))
            {
                string farktanSonrakiKelime = GetNextWord(str1.Substring(farkIndeks), str2.Substring(farkIndeks));
                return farktanSonrakiKelime;
            }
            else
            {
                return "";
            }
        }
        //End of Section11

        // Girilen iki stringten farklı olan bulan metod Section12
        private static string GetNextWord(string str1, string str2)
        {

            string[] words1 = str1.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string[] words2 = str2.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            
            if (words1.Length > 0 && words2.Length > 0)
            {
                return words1[0];
            }
            else
            {
                return "";
            }
        }
        // End of Section12


        // Girilen metindeki oşlukları kaldıran metod Section13
        private static string RemoveSpaces(string text)
        {
            text = text.Replace(" ", "");
            return text;
        }
        //End of Section13

    }

}
