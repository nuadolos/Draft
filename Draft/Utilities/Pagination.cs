using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Draft.Utilities
{
    internal class Pagination
    {
        #region Состояние класса

        private int index;
        private readonly int countOut;
        private int[] testPagesMas;

        #endregion

        #region Свойства

        public int NumPage { get; set; }

        public int TotalElement { get; set; }

        public int LengthMas
        {
            get
            {
                return this.testPagesMas.Length;
            }
        }

        public int TotalPages
        {
            get => (int)Math.Ceiling((decimal)TotalElement / countOut);
        }

        public bool HasPreviousPage
        {
            get => NumPage > 0;
        }

        public bool HasNextPage
        {
            get => NumPage < testPagesMas.Length - 1;
        }

        #endregion

        #region Конструктор

        public Pagination(int totalElemetn, int countOut = 20)
        {
            NumPage = 0;
            index = 0;
            this.countOut = countOut;
            TotalElement = totalElemetn;
            testPagesMas = CreateMasPages();
        }

        #endregion

        #region Методы

        public List<T> LimitedList<T>(List<T> tempList) => tempList.Count != 0 ? tempList.GetRange(index, testPagesMas[NumPage]) : null;

        public void GoBack()
        {
            NumPage--;
            index -= countOut;
        }

        public void GoForward()
        {
            NumPage++;
            index += countOut;
        }

        public void Zeroing()
        {
            NumPage = 0;
            index = 0;
        }

        public void IsSorted(int countEl)
        {
            if (TotalElement != countEl)
            {
                TotalElement = countEl;
                testPagesMas = CreateMasPages();
            }
        }

        public int[] CreateMasPages()
        {
            int[] tempPagesMas = new int[TotalPages];
            int tempTotalEl = TotalElement;

            for (int i = 0; i < tempPagesMas.Length; i++)
            {
                if (tempTotalEl - countOut > 0)
                {
                    tempPagesMas[i] = countOut;
                    tempTotalEl -= countOut;
                }
                else
                {
                    tempPagesMas[i] = tempTotalEl;
                    break;
                }
            }

            return tempPagesMas;
        }

        public void GetIndex()
        {
            index = countOut * NumPage;
        }

        public void Definition(params object[] buttons)
        {
            int tempNumPage = NumPage + 3;

            for (int i = 0; i < 3; i++)
            {
                if (tempNumPage > TotalPages)
                    tempNumPage--;
                else
                {
                    for (int j = buttons.Length - 1; j >= 0; j--)
                    {
                        if (buttons[j] is Button tempButton)
                        {
                            tempButton.Content = $"{tempNumPage--}";
                            if (int.Parse(tempButton.Content.ToString()) <= 0)
                                tempButton.Visibility = System.Windows.Visibility.Collapsed;
                            else
                                tempButton.Visibility = System.Windows.Visibility.Visible;
                        }
                    }
                    break;
                }
            }
        }

        #endregion
    }
}
