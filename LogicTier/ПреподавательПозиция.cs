using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTier;

namespace LogicTier
{
    public class ПреподавательПозиция
    {
        private Преподаватель _преподаватель;

        public ПреподавательПозиция(Преподаватель p)
        {
            _преподаватель = p;
        }

        public string ФИО
        {
            get { return _преподаватель.ФИО; }
            set { _преподаватель.ФИО = value; }
        }

        public string Должность
        {
            get { return _преподаватель.Должность; }
            set { _преподаватель.Должность = value; }
        }

        public string Кафедра
        {
            get { return _преподаватель.Кафедра; }
            set { _преподаватель.Кафедра = value; }
        }

        public decimal Зарплата
        {
            get { return _преподаватель.Зарплата; }
            set { _преподаватель.Зарплата = value; }
        }

        public string ПредставлениеПреподавателя
        {
            get { return _преподаватель.ПредставлениеПреподавателя; }
        }
    }
}
