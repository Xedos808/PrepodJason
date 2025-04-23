using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicTier
{
    public class Университет
    {
        private List<ПреподавательПозиция> _преподавателиПозиции;

        public Университет(List<ПреподавательПозиция> позиции)
        {
            _преподавателиПозиции = позиции;
        }

        public List<ПреподавательПозиция> СписокПреподавателей
        {
            get { return _преподавателиПозиции; }
        }

        public string НаименованиеУниверситета
        {
            get { return "Наш университет"; }
        }

        public int КоличествоПреподавателей
        {
            get
            {
                return _преподавателиПозиции
                    .OrderBy(p => p.Должность)
                    .Count(p => p.Должность.Equals("сч. преподаватель", StringComparison.OrdinalIgnoreCase));
            }
        }
        public Dictionary<string, decimal> СуммаЗарплатПоКафедрам
        {
            get
            {
                return _преподавателиПозиции
                    .Where(p => p.Должность.Equals("сч. преподаватель", StringComparison.OrdinalIgnoreCase))
                    .GroupBy(p => p.Кафедра)
                    .ToDictionary(g => g.Key, g => g.Sum(p => p.Зарплата));
            }
        }
    }
}
