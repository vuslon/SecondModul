using System;
using System.Collections.Generic;

/* Очередь с приоритетом на основе Heap Sort.
В данной реализации меньшее значение ключа TKey соответствует более высокому приоритету.
Релизованы два стандартных метода: Enqueue и Dequeue.
Также добавлен метод удаления элемента по заданной позиции(индекс +1) на базе метода Enqueue;

Добавлен метод Print() для вывода последовательности элементов по приоритету
*/

namespace PriorityQueueLastEdition
{
    class PriorityQueue<TKey, TValue>
    {
        private List<KeyValuePair<TKey, TValue>> _heap;
        private IComparer<TKey> _comparer;
        public PriorityQueue() : this(Comparer<TKey>.Default)
        {
        }

        public PriorityQueue(IComparer<TKey> comparer)
        {
            _heap = new List<KeyValuePair<TKey, TValue>>();
            _comparer=comparer;
        }

// компаратор, выделен в отдельный методом чтобы не повторяться с набором строки (той, что в теле данного метода)
        private int _comp(int x, int y) 
        {
            return _comparer.Compare(_heap[x].Key, _heap[y].Key);
        }

//Стандартный метод вставки в очередь: добаявлет элемент в конец и производит просеивание на верх
        public void Enqueue(TKey key, TValue value)
        {
            KeyValuePair<TKey, TValue> tmp = new KeyValuePair<TKey, TValue>(key, value);
            _heap.Add(tmp);
            SiftUp();
        }
     
     // Просеивание вверх по ключу TKey.    
        private void SiftUp()
        {
            int cur = _heap.Count-1;
            while (cur > 0)
            {
                int parents = (cur - 1)/2;
                if (_comp(cur,parents) < 0) // Если значение TKey потомка ниже - производится обмен элементов между позициями
                    Swap(cur, parents);
                else break; //Если значение TKey потомка выше - выход из метода

                cur = parents;
            }
        }
        //элементов между позициями
        private void Swap(int cur, int parents)
        {
            KeyValuePair<TKey, TValue> tmp = _heap[parents];
            _heap[parents] = _heap[cur];
            _heap[cur] = tmp;
        }
// Стандартный метод - удаляет первый элемент из кучи,  возвращает значение пары
        public KeyValuePair<TKey, TValue> Dequeue()
        {
            KeyValuePair<TKey, TValue> tmp = _dequeue(0);
            return tmp;
        }
        
// Метод для удаления элемента по указанной позиции, возвращает значение пары 
// принимает из метода Dequeue значение 0 - первый элемент,принимает из метода Delete значение указанное пользователем
        private KeyValuePair<TKey,TValue> _dequeue(int pos)
        {
            KeyValuePair<TKey, TValue> result = _heap[pos];
            _heap[pos] = _heap[_heap.Count-1];
            _heap.RemoveAt(_heap.Count-1);
            SiftDown(pos);
            return result;
        }

        //Метод для удаления пары по укаанной позиции
        public void Delete(int pos)
        {
            _dequeue(pos-1);
        }

        //Просеивание кучи вниз.
        private void SiftDown(int cur)
        {
            int child = cur * 2 + 2;

            while (child -1< _heap.Count)
            {
                if (child == _heap.Count) // рассматривается случай, когда родитель имеет только одну левую ветку
                    {
                        if (_comp(cur,child - 1) > 0)
                            Swap(cur, child - 1);
                        break;
                    }

                if (_comp(child - 1, child) < 0) // если две ветки, выбираем ветку с меньшим значением 
                    --child;

                if (_comp(cur, child) < 0) //выходим, если значение находятся в верном порядке (родитель меньше потомка)
                    break;

                Swap(cur, child);
                cur = child;
                child = cur*2 + 2;
            }
            
        }

// Вывод на консоль Очереди в правильном порядке по приоритету
        public void Print()
        {
            List<KeyValuePair<TKey, TValue>> tmpList = new List<KeyValuePair<TKey, TValue>>();
            int i = 1;
            while (_heap.Count > 0)
            {
                KeyValuePair<TKey, TValue> tmp = Dequeue();
                Console.WriteLine(i++ +":\t" + tmp);
                tmpList.Add(tmp); //так как к Листу значения прикрепляются в конец, Лист станет отсортированным. 
            //принципы сортировки Бинарной кучи не будут нарушены - необходимости добавления в начало и сортировки SiftDown нет
            }
            _heap = tmpList; 
        }
    }
}