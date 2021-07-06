using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Document.NET;

namespace calcNet
{
    class MakeWord
    {
        
        //enum Gosts
        //{
        //    GOST_34233_1 = 1,
        //    GOST_34233_2 = 2,
        //    GOST_34233_3 = 3,
        //    GOST_34233_4 = 4,
        //    GOST_34233_5 = 5,
        //    GOST_34233_6 = 6,
        //    GOST_34233_7 = 7,
        //    GOST_34233_8 = 8,
        //    GOST_34233_9 = 9,
        //    GOST_34233_10 = 10,
        //    GOST_34233_11 = 11
        //}

        internal static void MakeLit(List<int> bibliography, string Docum = null)
        {
            
                

            const string GOST_34233_1 = "ГОСТ 34233.1-2017 Сосуды и аппараты. Нормы и методы расчета на прочность. Общие требования";
            const string GOST_34233_2 = "ГОСТ 34233.2-2017 Сосуды и аппараты. Нормы и методы расчета на прочность. " +
                                        "Расчет цилиндрических и конических обечаек, выпуклых и плоских днищ и крышек";
            const string GOST_34233_3 = "ГОСТ 34233.3-2017 Сосуды и аппараты. Нормы и методы расчета на прочность. " +
                                        "Укрепление отверстий в обечайках и днищах при внутреннем и наружном давлениях. " +
                                        "Расчет на прочность обечаек и днищ при внешних статических нагрузках на штуцер";
            const string GOST_34233_4 = "ГОСТ 34233.4-2017 Сосуды и аппараты. Нормы и методы расчета на прочность. " +
                                        "Расчет на прочность и герметичность фланцевых соединений";
            const string GOST_34233_5 = "ГОСТ 34233.5-2017 Сосуды и аппараты. Нормы и методы расчета на прочность. " +
                                        "Расчет  обечаек и днищ от воздействия опорных нагрузок";
            const string GOST_34233_6 = "ГОСТ 34233.6-2017 Сосуды и аппараты. Нормы и методы расчета на прочность. " +
                                        "Расчет на прочность при малоцикловых нагрузках";
            const string GOST_34233_7 = "ГОСТ 34233.7-2017 Сосуды и аппараты. Нормы и методы расчета на прочность. " +
                                        "Теплообменные аппараты";
            const string GOST_34233_8 = "ГОСТ 34233.8-2017 Сосуды и аппараты. Нормы и методы расчета на прочность. " +
                                        "Сосуды и аппараты с рубашками";
            const string GOST_34233_9 = "ГОСТ 34233.9-2017 Сосуды и аппараты. Нормы и методы расчета на прочность. " +
                                        "Аппараты колонного типа";
            const string GOST_34233_10 = "ГОСТ 34233.10-2017 Сосуды и аппараты. Нормы и методы расчета на прочность. " +
                                        "Сосуды и аппараты работающие с сероводородными средами";
            const string GOST_34233_11 = "ГОСТ 34233.11-2017 Сосуды и аппараты. Нормы и методы расчета на прочность. " +
                                        "Метод расчета на прочность обечаек и днищ с учетом смещения кромок сварных соединений, " +
                                        "угловатости и некруглости обечаек";

            

            if (bibliography != null)
            {
                var doc = Xceed.Words.NET.DocX.Load(Docum);
                doc.InsertParagraph().InsertPageBreakAfterSelf();
                doc.InsertParagraph("Литература")
                   .Heading(HeadingType.Heading1)
                   .Color(System.Drawing.Color.Black).Alignment = Alignment.center;
                doc.InsertParagraph("1. " + GOST_34233_1);

                List<int> bibliographySort = bibliography.Distinct()
                                                         .ToList();
                bibliographySort.Sort();

                string[] Gosts = new string[] { "", GOST_34233_1, GOST_34233_2, GOST_34233_3, GOST_34233_4, GOST_34233_5,
                                            GOST_34233_6, GOST_34233_7, GOST_34233_8, GOST_34233_9, GOST_34233_10,
                                            GOST_34233_11 };

                for (int i = 0; i < bibliographySort.Count; i++)
                {
                    doc.InsertParagraph($"{i + 1}. " + Gosts[bibliographySort[i]]);

                    //switch (bibliographySort[i])
                    //{
                    //    case 1:
                    //        doc.InsertParagraph($"{i + 1}. " + GOST_34233_1);
                    //        break;
                    //    case 2:
                    //        doc.InsertParagraph($"{i + 1}. " + GOST_34233_2);
                    //        break;
                    //    case 3:
                    //        doc.InsertParagraph($"{i + 1}. " + GOST_34233_3);
                    //        break;
                    //    case 4:
                    //        doc.InsertParagraph($"{i + 1}. " + GOST_34233_4);
                    //        break;
                    //    case 5:
                    //        doc.InsertParagraph($"{i + 1}. " + GOST_34233_5);
                    //        break;
                    //    case 6:
                    //        doc.InsertParagraph($"{i + 1}. " + GOST_34233_6);
                    //        break;
                    //    case 7:
                    //        doc.InsertParagraph($"{i + 1}. " + GOST_34233_7);
                    //        break;
                    //    case 8:
                    //        doc.InsertParagraph($"{i + 1}. " + GOST_34233_8);
                    //        break;
                    //    case 9:
                    //        doc.InsertParagraph($"{i + 1}. " + GOST_34233_9);
                    //        break;
                    //    case 10:
                    //        doc.InsertParagraph($"{i + 1}. " + GOST_34233_10);
                    //        break;
                    //    case 11:
                    //        doc.InsertParagraph($"{i + 1}. " + GOST_34233_11);
                    //        break;
                    //}
                }

                doc.Save();
            }
        }

    }
}
