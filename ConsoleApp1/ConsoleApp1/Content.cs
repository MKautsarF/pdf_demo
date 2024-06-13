using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Geom;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;

namespace pdf_demo
{
    internal class Content
    {
        
        public Content(string fileName)
        {
            //var directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"C:\Train Simulator\Data\penilaian\PDF\");
            string directoryPath = @"C:\Train Simulator\Data\penilaian\PDF\";
            Directory.CreateDirectory(directoryPath);

            // Assuming fileName contains the desired name of your PDF file (without extension)
            string fullFilePath = System.IO.Path.Combine(directoryPath, fileName + ".pdf");

            // Must have write permissions to the path folder
            PdfWriter writer = new PdfWriter(fullFilePath);

            PdfDocument pdf = new PdfDocument(writer);

            Document document = new Document(pdf, PageSize.A4, false);
            // Replace this JSON data with your actual JSON data
            dynamic data = ReadJSON.ReadJsonFile(fileName);

            CreateHeader(document, data);
            document.Add(new Paragraph("\n"));
            CreatePreBody(document, data);
            document.Add(new Paragraph("\n"));
            CreateBody(document, data);
            //document.Add(new Paragraph("\n"));
            //CreateAfterBody(document, data);
            document.Add(new Paragraph("\n"));
            CreateFooter(document, data, pdf);

            document.Close();
        }

        public void CreateHeader(Document document, dynamic data)
        {
            Paragraph header = new Paragraph("Hasil Simulasi: " + (data["train_type"]).ToString())
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(20);

            Table subheaderTable = new Table(new float[] { 0.25f, 0.25f, 0.25f, 0.25f })
                .SetWidth(UnitValue.CreatePercentValue(100))  // Set the table width to 100%
                .SetTextAlignment(TextAlignment.CENTER)
                .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                .SetFontSize(12);  // Set the table borderless

            // Add four different texts to the subheader table
            subheaderTable.AddCell(new Cell().Add(new Paragraph("Mulai: " + (data["waktu_mulai"]).ToString())).SetBorder(Border.NO_BORDER));
            subheaderTable.AddCell(new Cell().Add(new Paragraph("Selesai: " + (data["waktu_selesai"]).ToString())).SetBorder(Border.NO_BORDER));
            subheaderTable.AddCell(new Cell().Add(new Paragraph((data["durasi"]).ToString())).SetBorder(Border.NO_BORDER));
            subheaderTable.AddCell(new Cell().Add(new Paragraph("Tanggal: " + (data["tanggal"]).ToString())).SetBorder(Border.NO_BORDER));

            LineSeparator line = new LineSeparator(new SolidLine(1f));

            document.Add(header);
            document.Add(line);
            document.Add(subheaderTable);
        }
        public void CreatePreBody(Document document, dynamic data)
        {
            Paragraph prebodyHeader = new Paragraph("Data Diri")
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(16);

            float[] columnWidths = { 1, 90, 200 };
            Table table = new Table(UnitValue.CreatePercentArray(columnWidths));
            int number = 0;

            for (int i = 0; i < 2; i++)
            {
                Cell[] headerFooter =
                {
                    new Cell().SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceGray(0.8f)).Add(new Paragraph("No.")),
                    new Cell().SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceGray(0.8f)).Add(new Paragraph("Data Diri")),
                    new Cell().SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceGray(0.8f)).Add(new Paragraph("Keterangan"))
                };

                foreach (Cell hfCell in headerFooter)
                {
                    if (i == 0)
                    {
                        table.AddHeaderCell(hfCell);
                    }
                    else
                    {
                        //table.AddFooterCell(hfCell);
                    }
                }
            }

            for (int counter = 0; counter < 8; counter++)
            {
                table.AddCell(new Cell().SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph((number + 1).ToString())));

                // Construct the key dynamically based on the counter value
                string key;
                switch (counter + 1)
                {
                    case 1:
                        key = "Nama Crew";
                        break;
                    case 2:
                        key = "Kedudukan";
                        break;
                    case 3:
                        key = "Usia";
                        break;
                    case 4:
                        key = "Kode Kedinasan";
                        break;
                    case 5:
                        key = "Nomor KA";
                        break;
                    case 6:
                        key = "Lintas";
                        break;
                    case 7:
                        key = "Nama Instruktur";
                        break;
                    case 8:
                        key = "Keterangan";
                        break;
                    // Add more cases as needed
                    default:
                        key = "DefaultKey" + (counter + 1); // You can modify this logic based on your requirements
                        break;
                }

                // Access the JSON data using the dynamically constructed key
                string value = key;

                table.AddCell(new Cell().SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph( value )));

                // Construct the key dynamically based on the counter value
                string key2;
                switch (counter + 1)
                {
                    case 1:
                        key2 = "nama_crew";
                        break;
                    case 2:
                        key2 = "kedudukan";
                        break;
                    case 3:
                        key2 = "usia";
                        break;
                    case 4:
                        key2 = "kode_kedinasan";
                        break;
                    case 5:
                        key2 = "no_ka";
                        break;
                    case 6:
                        key2 = "lintas";
                        break;
                    case 7:
                        key2 = "nama_instruktur";
                        break;
                    case 8:
                        key2 = "keterangan";
                        break;
                    // Add more cases as needed
                    default:
                        key2 = "DefaultKey" + (counter + 1); // You can modify this logic based on your requirements
                        break;
                }

                // Access the JSON data using the dynamically constructed key
                string value2 = data[key2];

                table.AddCell(new Cell().SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(value2)));
                number++;
            }

            document.Add(prebodyHeader);
            document.Add(table);

        }
        
        public void CreateBody(Document document, dynamic data)
        {
            Paragraph bodyHeader = new Paragraph("Penilaian")
                .SetTextAlignment(TextAlignment.LEFT)
                //.SetUnderline(1.5f , 3.5f) // strikethrough
                .SetFontSize(16);

            document.Add(bodyHeader);

            int totalUnitKompetensi = data["penilaian"].Count;
            List<int> nilaiList = new List<int>();
            List<int> bobotPoinList = new List<int>();
            List<int> bobotLKList = new List<int>();
            List<double> nilaiListUnit = new List<double>();
            List<double> nilaiListAll = new List<double>();

            for (int i = 0; i < (totalUnitKompetensi); i++)
            {
                // i untuk unit kompetensi

                // tabel unit kompetensi
                float[] columnWidths = { 30, 25, 100 };
                Table table = new Table(UnitValue.CreatePercentArray(columnWidths));
                int number = 0;
                int numberOuter = 0;

                // cek unit kompetensi disable
                bool unitDisable = data["penilaian"][i]["disable"];
                if (unitDisable == false)
                {
                    for (int y = 0; y < 2; y++)
                    // y untuk tabel unit kompetensi
                    {
                        Cell[] headerFooter =
                        {
                        new Cell().SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBackgroundColor(new DeviceGray(0.8f)).Add(new Paragraph( "Unit Kompetensi No. " + ((i+1).ToString()) )),
                        new Cell().SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBackgroundColor(new DeviceGray(0.8f)).Add(new Paragraph("Judul Unit")),
                        new Cell().SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE).Add(new Paragraph((data["penilaian"][i]["judul"]).ToString()))
                    };

                        foreach (Cell hfCell in headerFooter)
                        {
                            if (y == 0)
                            {
                                table.AddHeaderCell(hfCell);
                            }
                            else
                            {
                                //table.AddFooterCell(hfCell);
                            }
                        }
                    }
                    document.Add(table);
                    document.Add(new Paragraph("\n"));

                    // tabel Langkah kerja
                    float[] columnWidths2 = { 1, 60, 80, 20 };
                    Table table2 = new Table(UnitValue.CreatePercentArray(columnWidths2));

                    for (int z = 0; z < 2; z++)
                    // z untuk tabel langkah kerja
                    {
                        Cell[] headerFooter2 =
                        {
                        new Cell().SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceGray(0.8f)).Add(new Paragraph("No.")),
                        new Cell().SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceGray(0.8f)).Add(new Paragraph("Langkah kerja")),
                        new Cell().SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceGray(0.8f)).Add(new Paragraph("Poin Observasi")),
                        new Cell().SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceGray(0.8f)).Add(new Paragraph("Penilaian")) // Add the fourth column
                    };

                        foreach (Cell hfCell in headerFooter2)
                        {
                            if (z == 0)
                            {
                                table2.AddHeaderCell(hfCell);
                            }
                            else
                            {
                                //table.AddFooterCell(hfCell);
                            }
                        }
                    }
                    nilaiListUnit = new List<double>();
                    bobotLKList = new List<int>();
                    int totalLangkahKerja = data["penilaian"][i]["data"].Count;
                    for (int a = 0; a < totalLangkahKerja; a++)
                    {
                        int b = 0;
                        int totalPoin = data["penilaian"][i]["data"][a]["poin"].Count;
                        int nilaiLKBobot = 1;
                        // cek langkah kerja disable
                        bool langkahkerjaDisable = data["penilaian"][i]["data"][a]["disable"];
                        if (langkahkerjaDisable == false)
                        {
                            table2.AddCell(new Cell(totalPoin, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph((number + 1).ToString()))); // merge this cell
                            table2.AddCell(new Cell(totalPoin, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph((data["penilaian"][i]["data"][a]["langkah_kerja"]).ToString())));
                            // calculate nilai times bobot
                            nilaiLKBobot = Convert.ToInt32((data["penilaian"][i]["data"][a]["bobot"]));
                        }
                        else if (langkahkerjaDisable == true)
                        {
                            table2.AddCell(new Cell(totalPoin, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph((number + 1).ToString())).SetUnderline(1.5f, 3.5f)); // merge this cell
                            table2.AddCell(new Cell(totalPoin, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph((data["penilaian"][i]["data"][a]["langkah_kerja"]).ToString())).SetUnderline(1.5f, 3.5f));
                            // calculate nilai times bobot
                            nilaiLKBobot = 0;
                        }

                        double averageValue;
                        int bobotPoinAvg = 1;

                        nilaiList = new List<int>();
                        bobotPoinList = new List<int>();
                        for (b = 0; b < totalPoin; b++)
                        {
                            // cek poin disable
                            bool poinDisable = data["penilaian"][i]["data"][a]["poin"][b]["disable"];
                            if (poinDisable == false)
                            {
                                table2.AddCell(new Cell().SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph((data["penilaian"][i]["data"][a]["poin"][b]["observasi"]).ToString())));

                                table2.AddCell(new Cell().SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE).Add(new Paragraph((data["penilaian"][i]["data"][a]["poin"][b]["nilai"]).ToString())));
                            }
                            else if (poinDisable == true)
                            {
                                table2.AddCell(new Cell().SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph((data["penilaian"][i]["data"][a]["poin"][b]["observasi"]).ToString())).SetUnderline(1.5f, 3.5f));

                                table2.AddCell(new Cell().SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE).Add(new Paragraph((data["penilaian"][i]["data"][a]["poin"][b]["nilai"]).ToString())).SetUnderline(1.5f, 3.5f));
                            }

                            // cek nilai disable
                            bool nilaiDisable = data["penilaian"][i]["data"][a]["poin"][b]["disable"];
                            if (nilaiDisable == false)
                            {
                                int nilaiValue = Convert.ToInt32((data["penilaian"][i]["data"][a]["poin"][b]["nilai"]));
                                // calculate nilai times bobot
                                int nilaiPoinBobot = Convert.ToInt32((data["penilaian"][i]["data"][a]["poin"][b]["bobot"]));
                                nilaiValue = nilaiValue * nilaiPoinBobot;

                                nilaiList.Add(nilaiValue);
                                bobotPoinList.Add(nilaiPoinBobot);
                            }
                            else if (nilaiDisable == true)
                            {
                                //int nilaiValue = Convert.ToInt32((data["penilaian"][i]["data"][a]["poin"][b]["nilai"]));

                                //nilaiList.Add(nilaiValue);
                            }

                            if (b == (totalPoin - 1))
                            {

                                if (nilaiList.Count == 0)
                                {
                                    averageValue = 0;
                                }
                                else
                                {
                                    // Calculate the average value
                                    averageValue = nilaiList.Sum();
                                    // calculate total bobot
                                    bobotPoinAvg = bobotPoinList.Sum();

                                }

                                double ratarataPoin = averageValue / bobotPoinAvg;

                                // Add merged cells for the average value
                                table2.AddCell(new Cell(1, 3).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceGray(0.8f)).Add(new Paragraph("Rata-Rata Langkah Kerja " + (number + 1).ToString() + " : ")));
                                table2.AddCell(new Cell().SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(ratarataPoin.ToString("F2")))); // Display the average value with two decimal places

                                averageValue = ratarataPoin * nilaiLKBobot;

                                nilaiListUnit.Add(averageValue);
                                bobotLKList.Add(nilaiLKBobot);
                            }
                        }
                        number++;

                    }
                    numberOuter++;


                    double averageValueUnit = nilaiListUnit.Sum();
                    int bobotLKAvg = bobotLKList.Sum();

                    double ratarataLK = averageValueUnit / bobotLKAvg;
                    // Add merged cells for the average value
                    table2.AddCell(new Cell(1, 3).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceGray(0.65f)).Add(new Paragraph("Rata-Rata Unit Kompetensi No." + (i + 1).ToString() + " : ")));
                    table2.AddCell(new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(ratarataLK.ToString("F2"))));
                    document.Add(table2);

                    nilaiListAll.Add(ratarataLK);
                }

                else if (unitDisable == true)
                {
                    for (int y = 0; y < 2; y++)
                    // y untuk tabel unit kompetensi
                    {
                        Cell[] headerFooter =
                        {
                        new Cell().SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBackgroundColor(new DeviceGray(0.8f)).Add(new Paragraph( "Unit Kompetensi No. " + ((i+1).ToString()) )).SetUnderline(1.5f , 3.5f),
                        new Cell().SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBackgroundColor(new DeviceGray(0.8f)).Add(new Paragraph("Judul Unit")).SetUnderline(1.5f , 3.5f),
                        new Cell().SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE).Add(new Paragraph((data["penilaian"][i]["judul"]).ToString())).SetUnderline(1.5f , 3.5f)
                    };

                        foreach (Cell hfCell in headerFooter)
                        {
                            if (y == 0)
                            {
                                table.AddHeaderCell(hfCell);
                            }
                            else
                            {
                                //table.AddFooterCell(hfCell);
                            }
                        }
                    }
                    document.Add(table);
                    document.Add(new Paragraph("\n"));

                    // tabel Langkah kerja
                    float[] columnWidths2 = { 1, 60, 80, 20 };
                    Table table2 = new Table(UnitValue.CreatePercentArray(columnWidths2));

                    for (int z = 0; z < 2; z++)
                    // z untuk tabel langkah kerja
                    {
                        Cell[] headerFooter2 =
                        {
                        new Cell().SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceGray(0.8f)).Add(new Paragraph("No.")),
                        new Cell().SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceGray(0.8f)).Add(new Paragraph("Langkah kerja")),
                        new Cell().SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceGray(0.8f)).Add(new Paragraph("Poin Observasi")),
                        new Cell().SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceGray(0.8f)).Add(new Paragraph("Penilaian")) // Add the fourth column
                    };

                        foreach (Cell hfCell in headerFooter2)
                        {
                            if (z == 0)
                            {
                                table2.AddHeaderCell(hfCell);
                            }
                            else
                            {
                                //table.AddFooterCell(hfCell);
                            }
                        }
                    }
                    nilaiListUnit = new List<double>();
                    bobotLKList = new List<int>();
                    int totalLangkahKerja = data["penilaian"][i]["data"].Count;
                    for (int a = 0; a < totalLangkahKerja; a++)
                    {
                        int b = 0;
                        int totalPoin = data["penilaian"][i]["data"][a]["poin"].Count;
                        int nilaiLKBobot = 1;
                        // cek langkah kerja disable
                        bool langkahkerjaDisable = data["penilaian"][i]["data"][a]["disable"];
                        if (langkahkerjaDisable == false)
                        {
                            table2.AddCell(new Cell(totalPoin, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph((number + 1).ToString()))); // merge this cell
                            table2.AddCell(new Cell(totalPoin, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph((data["penilaian"][i]["data"][a]["langkah_kerja"]).ToString())));
                            // calculate nilai times bobot
                            nilaiLKBobot = Convert.ToInt32((data["penilaian"][i]["data"][a]["bobot"]));
                        }
                        else if (langkahkerjaDisable == true)
                        {
                            table2.AddCell(new Cell(totalPoin, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph((number + 1).ToString())).SetUnderline(1.5f, 3.5f)); // merge this cell
                            table2.AddCell(new Cell(totalPoin, 1).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph((data["penilaian"][i]["data"][a]["langkah_kerja"]).ToString())).SetUnderline(1.5f, 3.5f));
                            // calculate nilai times bobot
                            nilaiLKBobot = 0;
                        }

                        double averageValue;
                        int bobotPoinAvg = 1;

                        nilaiList = new List<int>();
                        bobotPoinList = new List<int>();
                        for (b = 0; b < totalPoin; b++)
                        {
                            // cek poin disable
                            bool poinDisable = data["penilaian"][i]["data"][a]["poin"][b]["disable"];
                            if (poinDisable == false)
                            {
                                table2.AddCell(new Cell().SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph((data["penilaian"][i]["data"][a]["poin"][b]["observasi"]).ToString())));

                                table2.AddCell(new Cell().SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE).Add(new Paragraph((data["penilaian"][i]["data"][a]["poin"][b]["nilai"]).ToString())));
                            }
                            else if (poinDisable == true)
                            {
                                table2.AddCell(new Cell().SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph((data["penilaian"][i]["data"][a]["poin"][b]["observasi"]).ToString())).SetUnderline(1.5f, 3.5f));

                                table2.AddCell(new Cell().SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE).Add(new Paragraph((data["penilaian"][i]["data"][a]["poin"][b]["nilai"]).ToString())).SetUnderline(1.5f, 3.5f));
                            }

                            // cek nilai disable
                            bool nilaiDisable = data["penilaian"][i]["data"][a]["poin"][b]["disable"];
                            if (nilaiDisable == false)
                            {
                                int nilaiValue = Convert.ToInt32((data["penilaian"][i]["data"][a]["poin"][b]["nilai"]));
                                // calculate nilai times bobot
                                int nilaiPoinBobot = Convert.ToInt32((data["penilaian"][i]["data"][a]["poin"][b]["bobot"]));
                                nilaiValue = nilaiValue * nilaiPoinBobot;

                                nilaiList.Add(nilaiValue);
                                bobotPoinList.Add(nilaiPoinBobot);
                            }
                            else if (nilaiDisable == true)
                            {
                                //int nilaiValue = Convert.ToInt32((data["penilaian"][i]["data"][a]["poin"][b]["nilai"]));

                                //nilaiList.Add(nilaiValue);
                            }

                            if (b == (totalPoin - 1))
                            {

                                if (nilaiList.Count == 0)
                                {
                                    averageValue = 0;
                                }
                                else
                                {
                                    // Calculate the average value
                                    averageValue = nilaiList.Sum();
                                    // calculate total bobot
                                    bobotPoinAvg = bobotPoinList.Sum();

                                }

                                double ratarataPoin = averageValue / bobotPoinAvg;

                                // Add merged cells for the average value
                                table2.AddCell(new Cell(1, 3).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceGray(0.8f)).Add(new Paragraph("Rata-Rata Langkah Kerja " + (number + 1).ToString() + " : ")));
                                table2.AddCell(new Cell().SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(ratarataPoin.ToString("F2")))); // Display the average value with two decimal places

                                averageValue = ratarataPoin * nilaiLKBobot;

                                nilaiListUnit.Add(averageValue);
                                bobotLKList.Add(nilaiLKBobot);
                            }
                        }
                        number++;

                    }
                    numberOuter++;


                    double averageValueUnit = nilaiListUnit.Sum();
                    int bobotLKAvg = bobotLKList.Sum();

                    //double ratarataLK = averageValueUnit / bobotLKAvg;

                    double ratarataLK = 0;

                    // Add merged cells for the average value
                    table2.AddCell(new Cell(1, 3).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceGray(0.65f)).Add(new Paragraph("Rata-Rata Unit Kompetensi No." + (i + 1).ToString() + " : ")));
                    table2.AddCell(new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(ratarataLK.ToString("F2"))));
                    document.Add(table2);

                    nilaiListAll.Add(ratarataLK);
                }

                document.Add(new Paragraph("\n"));

            }
            double averageValueAll = nilaiListAll.Average();

            document.Add(new Paragraph("\n"));
            CreateAfterBody(document, data, averageValueAll);
        }

        public void CreateAfterBody(Document document, dynamic data, double averageValueAll)
        {
            Paragraph afterbodyHeader = new Paragraph("Rata-Rata Total Nilai")
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(16);

            document.Add(afterbodyHeader);
            // tabel unit kompetensi
            float[] columnWidths = { 30, 50, 100 };
            Table table3 = new Table(UnitValue.CreatePercentArray(columnWidths));

            for (int i = 0; i < 2; i++)
            {
                Cell[] headerFooter =
                {
                        new Cell(1,2).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBackgroundColor(new DeviceGray(0.8f)).Add(new Paragraph( "Bagian Penilaian" )),
                        new Cell().SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBackgroundColor(new DeviceGray(0.8f)).Add(new Paragraph("Nilai"))
                    };

                foreach (Cell hfCell in headerFooter)
                {
                    if (i == 0)
                    {
                        table3.AddHeaderCell(hfCell);
                    }
                    else
                    {
                        //table.AddFooterCell(hfCell);
                    }
                }
            }

            for (int y = 0; y < 3; y++)
            {

                // Construct the key dynamically based on the counter value
                string key;
                switch (y + 1)
                {
                    case 1:
                        key = "Rata-Rata Total Penilaian Manual";
                        break;
                    case 2:
                        key = "Rata-Rata Total Penilaian Simulasi";
                        break;
                    case 3:
                        key = "Rata-Rata Total Semua Penilaian";
                        break;
                    // Add more cases as needed
                    default:
                        key = "DefaultKey" + (y + 1); // You can modify this logic based on your requirements
                        break;
                }

                // Access the JSON data using the dynamically constructed key
                string value = key;

                table3.AddCell(new Cell(1,2).SetTextAlignment(TextAlignment.LEFT).Add(new Paragraph(value)));

                double endGame = data["nilai_akhir"];
                double endGamePlus = (endGame + averageValueAll) / 2;

                // Construct the key dynamically based on the counter value
                string key2;
                switch (y + 1)
                {
                    case 1:
                        key2 = averageValueAll.ToString("F2");
                        break;
                    case 2:
                        key2 = (data["nilai_akhir"]).ToString("F2");
                        break;
                    case 3:
                        key2 = endGamePlus.ToString("F2");
                        break;
                    // Add more cases as needed
                    default:
                        key2 = "DefaultKey" + (y + 1); // You can modify this logic based on your requirements
                        break;
                }

                // Access the JSON data using the dynamically constructed key
                string value2 = key2;
                table3.AddCell(new Cell().SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(value2)));
            }
            document.Add(table3);
            document.Add(new Paragraph("\n"));
        }

        public void CreateFooter(Document document, dynamic data, PdfDocument pdf)
        {
            int n = pdf.GetNumberOfPages();
            for (int i = 1; i <= n; i++)
            {
                document.ShowTextAligned(new Paragraph(System.String
                   .Format("Halaman : " + i + " dari " + n)),
                   559 / 2, 20, i, TextAlignment.CENTER,
                   VerticalAlignment.BOTTOM, 0);
            }


        }

    }
}
