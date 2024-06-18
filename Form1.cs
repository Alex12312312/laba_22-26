using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace laba_22_26
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.RowCount = 5;     // число строк
            dataGridView1.ColumnCount = 6;  // число столбцов
            dataGridView1.Rows[0].Cells[1].Value = "Простая 2-фазная";
            dataGridView1.Rows[1].Cells[1].Value = "Простая 1-фазная";
            dataGridView1.Rows[2].Cells[1].Value = "Естественная 2-фазная";
            dataGridView1.Rows[3].Cells[1].Value = "Естественная 1-фазная";
            dataGridView1.Rows[4].Cells[1].Value = "Слияние";
            dataGridView1.Rows[0].Cells[0].Value = true;
            dataGridView1.Rows[1].Cells[0].Value = true;
            dataGridView1.Rows[2].Cells[0].Value = true;
            dataGridView1.Rows[3].Cells[0].Value = true;
            dataGridView1.Rows[4].Cells[0].Value = true;
        }

        
        private bool Check_table(DataGridViewRow row)
        {
            if (!(bool)row.Cells[0].Value)
            {
                row.Cells[2].Value = null;
                row.Cells[3].Value = null;
                row.Cells[4].Value = null;
                row.Cells[5].Value = null;
                return false;
            }
            return true;
        }

        
        private void UpdateTable(int sravn, int perestan, int ResultTime, bool sorted, int row)
        {
            dataGridView1.Rows[row].Cells[2].Value = sravn;
            dataGridView1.Rows[row].Cells[3].Value = perestan;
            dataGridView1.Rows[row].Cells[4].Value = ResultTime;
            dataGridView1.Rows[row].Cells[5].Value = sorted ? "Да" : "Нет";
        }

        
        private bool CheckMas(int[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                if (array[i] > array[i + 1]) return false;
            }
            return true;
        }

       
        private void Simple_2f(int n, int[] array)
        {
            if (!Check_table(dataGridView1.Rows[0])) return;
            int sravn = 0;
            int perestan = 0;
            Stopwatch stopwatch = Stopwatch.StartNew();
            int[] FileB = new int[n];
            int[] FileC = new int[n];
            for (int series = 1; series < n; series *= 2)
            {
                int aIndex = 0;
                int bLength = 0;
                int cLength = 0;
                while (aIndex < n)
                {
                    for (int i = 0; i < series && aIndex < n; i++)
                    {
                        FileB[bLength++] = array[aIndex++];
                        perestan++;
                    }
                    for (int i = 0; i < series && aIndex < n; i++)
                    {
                        FileC[cLength++] = array[aIndex++];
                        perestan++;
                    }
                }
                int aLength = 0; 
                int bIndex = 0; 
                int cIndex = 0;
                while (bIndex < bLength && cIndex < cLength)
                {
                    int i = 0, j = 0;
                    while (bIndex < bLength && cIndex < cLength && i < series && j < series)
                    {
                        sravn++;
                        perestan++;
                        if (FileB[bIndex] < FileC[cIndex])
                        {
                            array[aLength++] = FileB[bIndex++];
                            i++;
                        }
                        else
                        {
                            array[aLength++] = FileC[cIndex++];
                            j++;
                        }
                    }
                    while (bIndex < bLength && i < series)
                    {
                        perestan++;
                        array[aLength++] = FileB[bIndex++];
                        i++;
                    }
                    while (cIndex < cLength && j < series)
                    {
                        perestan++;
                        array[aLength++] = FileC[cIndex++];
                        j++;
                    }
                }

                while (bIndex < bLength)
                {
                    perestan++;
                    array[aLength++] = FileB[bIndex++];
                }
                while (cIndex < cLength)
                {
                    perestan++;
                    array[aLength++] = FileC[cIndex++];
                }
            }
            int ResultTime = (int)stopwatch.ElapsedMilliseconds;
            bool sorted = CheckMas(array);
            UpdateTable(sravn, perestan, ResultTime, sorted, 0);
        }

        
        private void Simple_1f(int N, int[] array)
        {
            if (!Check_table(dataGridView1.Rows[1])) return;
            int sravn = 0;
            int perestan = 0;
            int[][] bc = new int[2][] { new int[N], new int[N] };
            int[][] de = new int[2][] { new int[N], new int[N] };
            int[] bcLengths = new int[] { 0, 0 };
            int[] deIndexes = new int[2];
            int series = 1;
            int i, j;
            int n = 0;
            int arrayIndex = 0;
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (arrayIndex < N)
            {
                for (i = 0; arrayIndex < N && i < series; i++)
                {
                    perestan++;
                    bc[n][bcLengths[n]++] = array[arrayIndex++];
                }
                n = 1 - n;
            }

            int bLength = bcLengths[0];
            int cLength = bcLengths[1];
            while (cLength > 0)
            {
                n = 0;
                deIndexes[0] = 0;
                deIndexes[1] = 0;
                int[] b = bc[0], c = bc[1], writeArray = de[n];
                int bIndex = 0;
                int cIndex = 0;
                ref int writeIndex = ref deIndexes[n];
                while (bIndex < bLength && cIndex < cLength)
                {
                    i = j = 0;
                    while (bIndex < bLength && cIndex < cLength && i < series && j < series)
                    {
                        sravn++;
                        perestan++;
                        if (b[bIndex] < c[cIndex])
                        {
                            writeArray[writeIndex++] = b[bIndex++];
                            i++;
                        }
                        else
                        {
                            writeArray[writeIndex++] = c[cIndex++];
                            j++;
                        }
                    }
                    while (bIndex < bLength && i < series)
                    {
                        perestan++;
                        writeArray[writeIndex++] = b[bIndex++];
                        i++;
                    }
                    while (cIndex < cLength && j < series)
                    {
                        perestan++;
                        writeArray[writeIndex++] = c[cIndex++];
                        j++;
                    }
                    n = 1 - n;
                    writeArray = de[n];
                    writeIndex = ref deIndexes[n];
                }

                while (bIndex < bLength)
                {
                    perestan++;
                    writeArray[writeIndex++] = b[bIndex++];
                }
                while (cIndex < cLength)
                {
                    perestan++;
                    writeArray[writeIndex++] = c[cIndex++];
                }
                series *= 2;
                (bc, de) = (de, bc);
                bLength = deIndexes[0];
                cLength = deIndexes[1];
            }
            array = bc[0];
            int ResultTime = (int)stopwatch.ElapsedMilliseconds;
            bool sorted = CheckMas(array);
            UpdateTable(sravn, perestan, ResultTime, sorted, 1);
        }

        
        private void natural_2f(int N, int[] array)
        {
            if (!Check_table(dataGridView1.Rows[2])) return;
            int sravn = 0;
            int perestan = 0;
            int[] FileB = new int[N];
            int[] FileC = new int[N];
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (true)
            {
                int aIndex = 0;
                int bLength = 0;
                int cLength = 0;
                int current;
                while (aIndex < N)
                {
                    do
                    {
                        current = array[aIndex++];
                        FileB[bLength++] = current;
                        sravn++;
                        perestan++;
                    } while (aIndex < N && current <= array[aIndex]);
                    sravn--;

                    if (aIndex < N)
                    {
                        do
                        {
                            current = array[aIndex++];
                            FileC[cLength++] = current;
                            sravn++;
                            perestan++;
                        } while (aIndex < N && current <= array[aIndex]);
                        sravn--;
                    }
                }

                if (cLength == 0)
                    break;

                int FileBCurrent, FileCCurrent;
                aIndex = 0;
                int bIndex = 0, cIndex = 0;
                while (bIndex < bLength && cIndex < cLength)
                {
                    sravn++;
                    if (FileB[bIndex] < FileC[cIndex])
                    {
                        perestan++;
                        FileBCurrent = FileB[bIndex++];
                        array[aIndex++] = FileBCurrent;
                        if (bIndex < bLength && FileBCurrent > FileB[bIndex])
                        {
                            do
                            {
                                perestan++;
                                sravn++;
                                FileCCurrent = FileC[cIndex++];
                                array[aIndex++] = FileCCurrent;
                            } while (cIndex < cLength && FileCCurrent <= FileC[cIndex]);
                        }
                    }
                    else
                    {
                        perestan++;
                        FileCCurrent = FileC[cIndex++];
                        array[aIndex++] = FileCCurrent;
                        if (cIndex < cLength && FileCCurrent > FileC[cIndex])
                        {
                            do
                            {
                                perestan++;
                                sravn++;
                                FileBCurrent = FileB[bIndex++];
                                array[aIndex++] = FileBCurrent;
                            } while (bIndex < bLength && FileBCurrent <= FileB[bIndex]);
                        }
                    }
                }
                while (bIndex < bLength)
                {
                    perestan++;
                    array[aIndex++] = FileB[bIndex++];
                }
                while (cIndex < cLength)
                {
                    perestan++;
                    array[aIndex++] = FileC[cIndex++];
                }
            }
            int ResultTime = (int)stopwatch.ElapsedMilliseconds;
            bool sorted = CheckMas(array);
            UpdateTable(sravn, perestan, ResultTime, sorted, 2);
        }

        private void natural_1f(int N, int[] array)
        {
            if (!Check_table(dataGridView1.Rows[3])) return;
            int sravn = 0;
            int perestan = 0;
            int[][] bc = new int[2][]
            {
                new int[N],
                new int[N]
            };
            int[][] de = new int[2][]
            {
                new int[N],
                new int[N]
            };
            int[] bcLengths = new int[2] { 0, 0 };
            int[] bcIndexes = new int[2] { 0, 0 };            
            int[] deIndexes = new int[2] { 0, 0 };
            Stopwatch stopwatch = Stopwatch.StartNew();
            int n = 0;
            int arrayIndex = 0;
            while (arrayIndex < N)
            {
                int[] currentArr = bc[n];
                ref int currentLength = ref bcLengths[n];
                int currentEl = array[arrayIndex++];
                perestan++;
                currentArr[currentLength++] = currentEl;
                sravn++;
                if (arrayIndex < N && currentEl > array[arrayIndex])
                    n = 1 - n;
            }

            int bLength = bcLengths[0];
            int cLength = bcLengths[1];
            while (cLength > 0)
            {
                int m = 0;
                n = 0;
                bcIndexes[0] = 0;
                bcIndexes[1] = 0;
                deIndexes[0] = 0;
                deIndexes[1] = 0;                
                ref int bIndex = ref bcIndexes[0];
                ref int cIndex = ref bcIndexes[1];
                bLength = bcLengths[0];
                cLength = bcLengths[1];
                int[] readArr = bc[m];
                ref int readIndex = ref bcIndexes[m];
                ref int readLength = ref bcLengths[m];
                int[] writeArr = de[n];
                ref int writeIndex = ref deIndexes[n];

                int[] x = new int[2] { bc[0][0], bc[1][0] };
                int[] y = new int[2];

                while (bIndex < bLength && cIndex < cLength)
                {
                    sravn++;
                    if (x[m] > x[1 - m])
                    {
                        m = 1 - m;
                        readArr = bc[m];
                        readIndex = ref bcIndexes[m];
                        readLength = ref bcLengths[m];
                    }
                    perestan++;
                    writeArr[writeIndex++] = x[m];
                    y[m] = readArr[++readIndex];
                    sravn++;
                    if (readIndex > readLength || x[m] > y[m])
                    {
                        m = 1 - m;
                        readArr = bc[m];
                        readIndex = ref bcIndexes[m];
                        readLength = ref bcLengths[m];
                        perestan++;
                        writeArr[writeIndex++] = x[m];
                        y[m] = readArr[++readIndex];
                        while (readIndex < readLength && x[m] <= y[m])
                        {
                            sravn++;
                            x[m] = y[m];
                            perestan++;
                            writeArr[writeIndex++] = x[m];
                            y[m] = readArr[++readIndex];
                        }
                        x[1 - m] = y[1 - m];
                        n = 1 - n;
                        writeArr = de[n];
                        writeIndex = ref deIndexes[n];
                    }
                    x[m] = y[m];
                }

                int[] b = bc[0];
                int[] c = bc[1];

                while (bIndex < bLength)
                {
                    perestan++;
                    writeArr[writeIndex++] = x[0];
                    y[0] = b[++bIndex];
                    sravn++;
                    if (x[0] > y[0])
                    {
                        n = 1 - n;
                        writeArr = de[n];
                        writeIndex = ref deIndexes[n];
                    }
                    x[0] = y[0];
                }
                while (cIndex < cLength)
                {
                    perestan++;
                    writeArr[writeIndex++] = x[1];
                    y[1] = c[++cIndex];
                    sravn++;
                    if (x[1] > y[1])
                    {
                        n = 1 - n;
                        writeArr = de[n];
                        writeIndex = ref deIndexes[n];
                    }
                    x[1] = y[1];
                }
                (bc, de) = (de, bc);
                bcLengths[0] = deIndexes[0];
                cLength = bcLengths[1] = deIndexes[1];
            }

            array = bc[0];
            int ResultTime = (int)stopwatch.ElapsedMilliseconds;
            bool sorted = CheckMas(array);
            UpdateTable(sravn, perestan, ResultTime, sorted, 3);
        }

        private void absorption(int N, int[] array)
        {
            if (!Check_table(dataGridView1.Rows[4])) return;
            int sravn = 0;
            int perestan = 0;
            int k = Convert.ToInt32(numericUpDown2.Value/ 100 * N);
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = N - k; i >= 0; i -= k)
            {
                int portionSize = Math.Min(k, N - i);
                int[] portion = new int[portionSize];
                Array.Copy(array, i, portion, 0, portionSize);
                Array.Sort(portion);

                int j = 0, m = 0, n = i;
                int current;
                while (j < portionSize && m < k)
                {
                    current = array[n + m];
                    sravn++;
                    if (current <= portion[j])
                    {
                        array[n + j + m - i] = current;
                        m++;
                    }
                    else
                    {
                        array[n + j + m - i] = portion[j];
                        j++;
                    }
                    perestan++;
                }

                // Если остались элементы в исходном массиве
                while (m < k)
                {
                    array[n + j + m - i] = array[n + m];
                    m++;
                    perestan++;
                }

                // Если остались элементы в порции данных
                while (j < portionSize)
                {
                    array[n + j + m - i] = portion[j];
                    j++;
                    perestan++;
                }
            }
            int ResultTime = (int)stopwatch.ElapsedMilliseconds;
            bool sorted = CheckMas(array);
            UpdateTable(sravn, perestan, ResultTime, sorted, 4);
        }

        private void Sorting_Click(object sender, System.EventArgs e)
        {
            int n = Convert.ToInt32(numericUpDown1.Value);
            Random random = new Random();
            int[] array = new int[n];
            for (int i = 0; i < n; i++) array[i] = random.Next(0, n);
            int[] newArr = new int[n];
            Array.Copy(array, newArr, 0);
            Simple_2f(n, newArr.ToArray());
            int[] newArr1 = new int[n];
            Array.Copy(array, newArr1, 0);
            Simple_1f(n, newArr1.ToArray());
            int[] newArr2 = new int[n];
            Array.Copy(array, newArr2, 0);
            natural_2f(n, newArr2.ToArray());
            int[] newArr3 = new int[n];
            Array.Copy(array, newArr3, 0);
            natural_1f(n, newArr3.ToArray());
            int[] newArr4 = new int[n];
            Array.Copy(array, newArr4, 0);
            absorption(n, newArr4.ToArray());
        }

        private void Exit_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}