using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scada.Comm.Devices.KpKBA;
using Scada.Data.Tables;

namespace Scada.Comm.Devices
{
    public sealed class Kp1CView : KPView
    {
        /// <summary>
        /// Конструктор для общей настройки библиотеки КП
        /// </summary>
        public Kp1CView()
            : this(0)
        {
        }

        /// <summary>
        /// Конструктор для настройки конкретного КП
        /// </summary>
        public Kp1CView(int number)
            : base(number)
        {
            CanShowProps = true;
        }


        /// <summary>
        /// Описание библиотеки КП
        /// </summary>
        public override string KPDescr
        {
            get
            {
                return Localization.UseRussian ? "Библиотека КП для Получения данных с 1С. \n\n" : "---";

                    
            }
        }

       

        public override void ShowProps()
        {
          //  if (Number > 0)
                // отображение формы настройки свойств КП
             //   FrmConfig.ShowDialog(AppDirs, Number);
            
        }
    }
}
