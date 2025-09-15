using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace kadrCheck
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            trackBarFrames.Scroll += trackBarFrames_Scroll;
        }



        private List<ushort[]> frames = new List<ushort[]>();

        private int cols = 32;//таблица информационной части
        private int rows = 16;

        public int margin = 8;//отступы
        public int leftMargin = 30;


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();//загрузка файлв
            ofd.Filter = "KDR файлы|*.kdr|Все файлы|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string text = File.ReadAllText(ofd.FileName);
                    ParseFrames(text);

                    if (frames.Count > 0)
                    {
                        trackBarFrames.Minimum = 0;
                        trackBarFrames.Maximum = frames.Count - 1;
                        trackBarFrames.Value = 0;
                        ShowFrame(0);
                    }
                    else
                    {
                        MessageBox.Show("Кадры не найдены");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка чтения файла: " + ex.Message);
                }
            }
        }
        private void ParseFrames(string text)
        {
            frames.Clear();

            Regex rx = new Regex(@"=KADR=\s+((?:[0-9A-Fa-f]{4}\s+){543})");// поиск групп по 543 HEX-слова // 4 символа + пробел
            MatchCollection matches = rx.Matches(text);

            foreach (Match m in matches)
            {
                string[] parts = m.Groups[1].Value.Split(
                    new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 543)
                {
                    ushort[] nums = new ushort[543];//преобразование в ushort из HEX
                    for (int i = 0; i < 543; i++)
                        nums[i] = Convert.ToUInt16(parts[i], 16);

                    frames.Add(nums);
                }
            }
        }
        
        private void trackBarFrames_Scroll(object sender, EventArgs e)
        {
            if (frames == null || frames.Count == 0) return;

            int idx = trackBarFrames.Value;
            if (idx < 0) idx = 0;
            if (idx >= frames.Count) idx = frames.Count - 1;

            ShowFrame(idx);

            
        }

        
        private void ShowFrame(int index)// Отрисовка кадра в pictureBoxFrame
        {

            if (frames == null || frames.Count == 0) return;
            if (index < 0 || index >= frames.Count) return;

            ushort[] frame = frames[index];
            if (frame == null || frame.Length < 543) return;

            int outW = pictureBoxFrame.ClientSize.Width;
            int outH = pictureBoxFrame.ClientSize.Height;
            if (outW <= 0 || outH <= 0) return;

            Bitmap bmp = new Bitmap(outW, outH);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;


                using (Font font = new Font("Consolas", 9))
                using (Font headerFont = new Font("Consolas", 10, FontStyle.Bold))//служебная часть кадра
                {
                    float y = margin;
                    g.DrawString("Служебная часть кадра", headerFont, Brushes.Black, margin + leftMargin, y);
                    float fHeight = g.MeasureString("A", font).Height;
                    y += fHeight + 6;


                    SizeF sample = g.MeasureString("FFFF", font);// кол-во слов в строке
                    float wordW = sample.Width + 8;
                    int wordsPerRow = Math.Max(1, (int)((outW - 2 * margin - leftMargin) / wordW));

                    for (int i = 0; i < 31; i++)
                    {
                        ushort svcVal10 = (ushort)(frame[i] & 0x03FF); // 10 младших бит
                        string hex = svcVal10.ToString("X3");
                        int row = i / wordsPerRow;
                        int col = i % wordsPerRow;
                        float x = margin + leftMargin + col * wordW;
                        float yy = y + row * (fHeight + 6);
                        g.DrawString(hex, font, Brushes.Blue, x, yy);
                    }


                    int svcRows = (31 + wordsPerRow - 1) / wordsPerRow;
                    y += svcRows * (fHeight + 6) + 16;


                    g.DrawString("Информационная часть кадра", headerFont, Brushes.Black, margin + leftMargin, y);// информационная часть
                    y += fHeight + 8;


                    float availableWidth = outW - 2 * margin - leftMargin;
                    float cellWidth = availableWidth / (float)cols;
                    float cellHeight = 20;

                    using (Pen gridPen = new Pen(Color.LightGray, 0.5f))
                    {
                        for (int col = 0; col < cols; col++)
                        {
                            float x = margin + leftMargin + col * cellWidth;
                            g.DrawString(col.ToString("X2"), font, Brushes.Gray, x + (cellWidth - g.MeasureString(col.ToString("X2"), font).Width) / 2, y);
                        }
                        y += cellHeight;

                        for (int row = 0; row < rows; row++) // 31-542
                        {
                            g.DrawString(row.ToString("X2"), font, Brushes.Gray, margin + 2, y + (cellHeight - fHeight) / 2);

                            for (int col = 0; col < cols; col++)
                            {
                                int idx = 31 + row * cols + col;
                                if (idx >= frame.Length) continue;

                                byte val = (byte)((frame[idx] >> 1) & 0xFF); // 8 бит данных
                                string hex = val.ToString("X2");

                                float x = margin + leftMargin + col * cellWidth;

                                float textX = x + (cellWidth - g.MeasureString(hex, font).Width) / 2;//центрирование текста
                                float textY = y + (cellHeight - fHeight) / 2;

                                g.DrawString(hex, font, Brushes.Black, textX, textY);

                                if (col < cols - 1)
                                {
                                    g.DrawLine(gridPen, x + cellWidth, y, x + cellWidth, y + cellHeight);
                                }
                            }

                            g.DrawLine(gridPen, margin + leftMargin, y + cellHeight, outW - margin, y + cellHeight);
                            y += cellHeight;

                            if (y + cellHeight > outH - margin)
                                break;
                        }

                        g.DrawRectangle(gridPen, margin + leftMargin, y - rows * cellHeight, availableWidth, rows * cellHeight);
                    }
                }
            }

            
        
    

              
            Image old = pictureBoxFrame.Image;// обновление кадров
            pictureBoxFrame.Image = bmp;
            if (old != null)
            {
                old.Dispose();
            }

            labelFrameNumber.Text = $"Кадр: {index + 1} / {frames.Count}";
            labelFrameNumber.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);


        }




    }

}
