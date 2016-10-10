using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeTransfer.controller;

namespace HomeTransfer.view
{
    class HomeTransferView
    {
        private static HomeTransferView view;

        public static HomeTransferView getInstance()
        {
            if (view == null)
            {
                view = new HomeTransferView();
            }
            return view;
        }

        public void init()
        {
            HomeTransferController.getInstance().initController();
            HomeTransferController.getInstance().broadcastUDP("discover");
        }

        public void exit()
        {
            HomeTransferController.getInstance().broadcastUDP("close");
            HomeTransferController.getInstance().exitController();
        }
    }
}
