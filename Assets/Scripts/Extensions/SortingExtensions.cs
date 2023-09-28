using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class SortingExtensions
    {
        #region Sort Methods

        public static T[] BubbleSort<T>(this T[] array) where T : IComparable<T>
        {
            int n = array.Length;
            bool swapped;

            do
            {
                swapped = false;
                for (int i = 1; i < n; i++)
                {
                    if (array[i - 1].CompareTo(array[i]) > 0)
                    {
                        (array[i], array[i - 1]) = (array[i - 1], array[i]);
                        swapped = true;
                    }
                }
                n--;
            } while (swapped);

            return array;
        }
        public static T[] HeapSort<T>(this T[] array) where T : IComparable<T>
        {

            int n = array.Length;
            for (int i = n / 2 - 1; i >= 0; i--)
            {
                Heapify(array, n, i);
            }

            for (int i = n - 1; i > 0; i--)
            {
                (array[i], array[0]) = (array[0], array[i]);
                Heapify(array, i, 0);
            }

            return array;
        }
        public static T[] QuickSort<T>(this T[] array) where T : IComparable<T>
        {
            QuickSortRecursive(array, 0, array.Length - 1);
            return array;
        }
        public static T[] MergeSort<T>(this T[] array) where T : IComparable<T>
        {

            MergeSortRecursive(array, 0, array.Length - 1);
            return array;
        }
        public static T[] SelectionSort<T>(this T[] array) where T : IComparable<T>
        {
            int n = array.Length;
            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (array[j].CompareTo(array[minIndex]) < 0)
                    {
                        minIndex = j;
                    }
                }
                if (minIndex != i)
                {
                    (array[i], array[minIndex]) = (array[minIndex], array[i]);
                }
            }
            return array;
        }
        public static T[] InsertionSort<T>(this T[] array) where T : IComparable<T>
        {
            int n = array.Length;
            for (int i = 1; i < n; i++)
            {
                T key = array[i];
                int j = i - 1;

                while (j >= 0 && array[j].CompareTo(key) > 0)
                {
                    array[j + 1] = array[j];
                    j--;
                }

                array[j + 1] = key;
            }

            return array;
        }
        public static T[] CountingSort<T>(this T[] array) where T : IComparable<T>
        {
            int n = array.Length;
            if (n <= 1)
            {
                return array;
            }

            T min = array[0];
            T max = array[0];
            for (int i = 1; i < n; i++)
            {
                if (array[i].CompareTo(min) < 0)
                {
                    min = array[i];
                }
                else if (array[i].CompareTo(max) > 0)
                {
                    max = array[i];
                }
            }

            int[] countArray = new int[max.GetHashCode() - min.GetHashCode() + 1];

            for (int i = 0; i < n; i++)
            {
                countArray[array[i].GetHashCode() - min.GetHashCode()]++;
            }

            int index = 0;
            for (int i = 0; i < countArray.Length; i++)
            {
                for (int j = 0; j < countArray[i]; j++)
                {
                    array[index] = (T)Convert.ChangeType(i + min.GetHashCode(), typeof(T));
                    index++;
                }
            }

            return array;
        }
        public static T[] BucketSort<T>(this T[] array) where T : IComparable<T>
        {
            int n = array.Length;
            if (n <= 1)
            {
                return array;
            }

            T min = array[0];
            T max = array[0];
            for (int i = 1; i < n; i++)
            {
                if (array[i].CompareTo(min) < 0)
                {
                    min = array[i];
                }
                else if (array[i].CompareTo(max) > 0)
                {
                    max = array[i];
                }
            }

            int bucketCount = (int)Math.Sqrt(n);

            List<T>[] buckets = new List<T>[bucketCount];
            for (int i = 0; i < bucketCount; i++)
            {
                buckets[i] = new List<T>();
            }

            for (int i = 0; i < n; i++)
            {
                int bucketIndex = (int)Math.Floor((double)(array[i].GetHashCode() - min.GetHashCode()) / (max.GetHashCode() - min.GetHashCode() + 1) * (bucketCount - 1));
                buckets[bucketIndex].Add(array[i]);
            }

            for (int i = 0; i < bucketCount; i++)
            {
                buckets[i].Sort();
            }

            int index = 0;
            for (int i = 0; i < bucketCount; i++)
            {
                for (int j = 0; j < buckets[i].Count; j++)
                {
                    array[index] = buckets[i][j];
                    index++;
                }
            }

            return array;
        }
        public static T[] RadixSort<T>(this T[] array) where T : IComparable<T>
        {
            int n = array.Length;
            if (n <= 1)
            {
                return array;
            }

            T max = array[0];
            for (int i = 1; i < n; i++)
            {
                if (array[i].CompareTo(max) > 0)
                {
                    max = array[i];
                }
            }

            int numDigits = max.GetHashCode().ToString().Length;

            int exp = 1;
            for (int i = 0; i < numDigits; i++)
            {
                CountingSortByDigit(array, n, exp);
                exp *= 10;
            }

            return array;
        }

        #endregion

        #region Auxiliary methods

        private static void CountingSortByDigit<T>(T[] array, int n, int exp) where T : IComparable<T>
        {
            T[] output = new T[n];
            int[] countArray = new int[10];

            for (int i = 0; i < n; i++)
            {
                int digit = (array[i].GetHashCode() / exp) % 10;
                countArray[digit]++;
            }

            for (int i = 1; i < 10; i++)
            {
                countArray[i] += countArray[i - 1];
            }

            for (int i = n - 1; i >= 0; i--)
            {
                int digit = (array[i].GetHashCode() / exp) % 10;
                output[countArray[digit] - 1] = array[i];
                countArray[digit]--;
            }

            for (int i = 0; i < n; i++)
            {
                array[i] = output[i];
            }
        }

        private static void MergeSortRecursive<T>(T[] array, int left, int right) where T : IComparable<T>
        {
            if (left < right)
            {
                int middle = left + (right - left) / 2;
                MergeSortRecursive(array, left, middle);
                MergeSortRecursive(array, middle + 1, right);
                Merge(array, left, middle, right);
            }
        }
        private static void Merge<T>(T[] array, int left, int middle, int right) where T : IComparable<T>
        {
            int n1 = middle - left + 1;
            int n2 = right - middle;

            T[] leftArray = new T[n1];
            T[] rightArray = new T[n2];

            for (int i = 0; i < n1; i++)
            {
                leftArray[i] = array[left + i];
            }

            for (int j = 0; j < n2; j++)
            {
                rightArray[j] = array[middle + 1 + j];
            }

            int k = left;
            int leftIndex = 0;
            int rightIndex = 0;

            while (leftIndex < n1 && rightIndex < n2)
            {
                if (leftArray[leftIndex].CompareTo(rightArray[rightIndex]) <= 0)
                {
                    array[k] = leftArray[leftIndex];
                    leftIndex++;
                }
                else
                {
                    array[k] = rightArray[rightIndex];
                    rightIndex++;
                }
                k++;
            }

            while (leftIndex < n1)
            {
                array[k] = leftArray[leftIndex];
                leftIndex++;
                k++;
            }

            while (rightIndex < n2)
            {
                array[k] = rightArray[rightIndex];
                rightIndex++;
                k++;
            }
        }

        private static void QuickSortRecursive<T>(T[] array, int left, int right) where T : IComparable<T>
        {
            if (left < right)
            {
                int pivotIndex = Partition(array, left, right);
                QuickSortRecursive(array, left, pivotIndex - 1);
                QuickSortRecursive(array, pivotIndex + 1, right);
            }
        }
        private static int Partition<T>(T[] array, int left, int right) where T : IComparable<T>
        {
            T pivotValue = array[right];
            int pivotIndex = left - 1;

            for (int i = left; i < right; i++)
            {
                if (array[i].CompareTo(pivotValue) <= 0)
                {
                    pivotIndex++;
                    (array[pivotIndex], array[i]) = (array[i], array[pivotIndex]);
                }
            }

            pivotIndex++;
            (array[pivotIndex], array[right]) = (array[right], array[pivotIndex]);
            return pivotIndex;
        }

        private static void Heapify<T>(T[] array, int n, int i) where T : IComparable<T>
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            if (left < n && array[left].CompareTo(array[largest]) > 0)
            {
                largest = left;
            }

            if (right < n && array[right].CompareTo(array[largest]) > 0)
            {
                largest = right;
            }

            if (largest != i)
            {
                (array[largest], array[i]) = (array[i], array[largest]);
                Heapify(array, n, largest);
            }
        }

        #endregion

        #region Documentation

        /*
        Bubble Sort:
        Description: A simple sorting algorithm that compares adjacent elements and swaps them if they are in the wrong order. Repeats this process until the array is sorted.
        Performance: Bubble Sort has a complexity of O(n^2), which makes it inefficient for sorting large arrays. It is rarely used, but can be useful for small arrays or almost sorted data.
       
        Heap Sort:
        Description: A sorting algorithm based on a heap tree. First, a max-heap (for ascending order) or min-heap (for descending order) is built, then items are removed from the heap one by one and inserted at the end of the sorted portion of the array.
        Performance: Heap Sort has a complexity of O(n log n) in all cases, which makes it efficient for sorting large arrays. It is used in many standard libraries and has good performance.
        
        Quick Sort:
        Characteristics: A recursive sorting algorithm that divides an array into smaller parts relative to a reference element and then sorts each part recursively. It uses a divide-and-conquer strategy.
        Performance: Quick Sort has the best average O(n log n) time analysis among the comparative sorts. However, its worst-case O(n^2) time analysis can be a problem for already sorted or almost sorted data. Quick Sort is usually a fast and efficient algorithm for many cases.
         
        Merge Sort:
        Definition: A sorting algorithm based on dividing an array into halves, sorting each half recursively, and then merging the sorted subarrays.
        Efficiency: Merge Sort has a stable O(n log n) time analysis regardless of the data, making it efficient for sorting large arrays. It usually has good performance, but requires additional memory to merge subarrays.
        
        Selection Sort:
        Characteristics: A simple sorting algorithm that looks for the smallest (or largest) element in an array and swaps it with the first (or last) element, then repeats this process for the next subarray.
        Efficiency: Selection Sort has a complexity of O(n^2), which makes it inefficient for large arrays. It is rarely used, but can be useful for small arrays or almost sorted data
         
        Insertion Sort:
        Description: A simple sorting algorithm that inserts each subsequent element in an array into an already sorted part of the array. For each new element, the correct position in the sorted part of the array is searched for.
        Efficiency: Insertion Sort has a complexity of O(n^2) in the worst and average cases, and O(n) in the best case for an already sorted array. It is often used for small arrays or as an enhancement to more efficient sorting algorithms.
         
        Counting Sort:
        Definition: A sorting algorithm that relies on counting the number of occurrences of each element in an array and using this information to restore an ordered array.
        Performance: Counting Sort has a complexity of O(n + k), where n is the number of elements in the array and k is the difference between the maximum and minimum elements. This makes Counting Sort very efficient for sorting arrays that have a limited range of values, particularly for integer data types. However, Counting Sort requires additional memory to create the array to be counted, and therefore is not suitable for large value ranges. It is used when the range of values of the array is small enough and known at the design stage of the algorithm.
        
        Bucket Sort:
        Description: A sorting algorithm that divides an array into a number of "buckets" or "pockets" and places each item in the appropriate bucket, depending on the value of the item. Each bucket is then individually sorted, for example, using an insertion sort or any other sorting algorithm.
        Efficiency: If the data is evenly distributed over a range from minimum to maximum value, Bucket Sort can be very efficient and have a complexity of O(n + k), where n is the number of items and k is the number of buckets. However, if the data is unevenly distributed, there may be cases when the algorithm is less efficient. Typically, Bucket Sort is used to sort numbers with a limited range of values or to sort fractional numbers.
         
        Radix Sort:
        Description: A sorting algorithm that sorts the elements of an array using their decimal digits, starting with the least significant digit and ending with the most significant. Each digit is sorted separately using a stable sort such as Counting Sort or Bucket Sort.
        Efficiency: Typically, Radix Sort has a complexity of O(d * (n + k)), where d is the number of bits, n is the number of elements, and k is the number of possible values on each bit. Radix sorting is effective for large data sets with a limited range of numbers or for sorting data with a fixed number of bits, such as integers or long numeric strings. Radix sorting can also be used to sort strings, particularly if the length of the string is fixed. 
         */

        #endregion
    }
}