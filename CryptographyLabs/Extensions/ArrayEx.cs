using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyLabs.Extensions
{
    public static class ArrayEx
    {
        public static T[][] Transform<T>(T[][] values, int newRowCount)
        {
            int itemsCount = 0;
            for (int i = 0; i < values.Length; i++)
                itemsCount += values[i].Length;

            int colCount = itemsCount / newRowCount;
            if (itemsCount % newRowCount != 0)
                colCount++;

            T[][] result = new T[newRowCount][];
            for (int row = 0; row < result.Length; row++)
                result[row] = new T[colCount];

            int index = 0;
            for (int i = 0; i < values.Length; i++)
            {
                for (int j = 0; j < values[i].Length; j++)
                {
                    result[index / colCount][index % colCount] = values[i][j];
                    index++;
                }
            }

            return result;
        }
    }
}
