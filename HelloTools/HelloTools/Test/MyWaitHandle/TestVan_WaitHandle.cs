using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace HelloTools.Test.MyWaitHandle
{
    class TestVan_WaitHandle
    {
        Random _random = null;// new Random(6);
        int _seed = -1; // seed.Next(5);
        int CurrentIdx = 0;
        Baby baby = new Baby("CaiCai");
        protected AutoResetEvent[] _ares = null;
        protected List<TBox> _boxes = null;
        delegate void DefDlgt_WhatsInBox<TGift, TReletive>(TGift gift, TBox box, TReletive fromWhom, Baby baby);
        //DefDlgt_WhatsInBox<GiftBase, Reletive> _dlgtWhatsInBox = null;

        public TestVan_WaitHandle()
        {
            _random = new Random(1);
            //_dlgtWhatsInBox = new DefDlgt_WhatsInBox<GiftBase, Reletive>(DlgtFunc_WhatsInBox);
        }

        void BuyGift(out GiftBase gift, out Reletive fromWhom)
        {
            switch (_seed = _random.Next(5))
            {
                case 0: { fromWhom = new Father("Baby's Dad"); break; }
                case 1: { fromWhom = new Grandpa("Baby's grandpa"); break; }
                case 2: { fromWhom = new Grandma("Baby's grandma"); break; }
                case 5: { fromWhom = new Baby("Baby"); break; }
                case 4:
                default: { fromWhom = new Mother("Baby's Mom"); break; }
            }
            switch (_seed = _random.Next(3))
            {
                case 0: { gift = new Gift_Bike("300", fromWhom.ToString()); break; }
                case 1: { gift = new Gift_Toy("15", fromWhom.ToString()); break; }
                case 2:
                default: { gift = new Gift_Book("75", fromWhom.ToString()); break; }
            }
        }

        public void Load()
        {
            System.Diagnostics.Trace.WriteLine(string.Format("[PID:{2}] {0}.{1}() begin", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Thread.CurrentThread.ManagedThreadId));
            //Random seed = new Random(1);
            int count = 5;// seed.Next(5);
            _boxes = new List<TBox>(count);
            for (int i = 0; i < count; i++)
            {
                TBox b = new TBox(i);
                _boxes.Add(b);
            }
            System.Diagnostics.Trace.WriteLine(string.Format("[PID:{2}] {0}.{1}() end", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Thread.CurrentThread.ManagedThreadId));
        }

        public void Run()
        {
            System.Diagnostics.Trace.WriteLine(string.Format("[PID:{2}] {0}.{1}() begin", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Thread.CurrentThread.ManagedThreadId));

            CurrentIdx = 0;
            GiftBase gift = null;
            Reletive fromWhom = null;

            BuyGift(out gift, out fromWhom);
            //System.Diagnostics.Trace.WriteLine(string.Format("[PID:{0}] A gift has been bought.", Thread.CurrentThread.ManagedThreadId));

            if (null != _ares) Array.Clear(_ares, 0, _ares.Length);
            _ares = _boxes.Select(x => new AutoResetEvent(false)).ToArray();

            var _dlgtWhatsInBox = new DefDlgt_WhatsInBox<GiftBase, Reletive>(DlgtFunc_WhatsInBox);
            _dlgtWhatsInBox.BeginInvoke(gift, _boxes[CurrentIdx], fromWhom, baby, Acb_WhatsInBox, _dlgtWhatsInBox);

            System.Diagnostics.Trace.WriteLine(string.Format("[PID:{2}] {0}.{1}() See what's in box...", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Thread.CurrentThread.ManagedThreadId));
            WaitHandle.WaitAll(_ares);

            System.Diagnostics.Trace.WriteLine(string.Format("[PID:{2}] {0}.{1}() end", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Thread.CurrentThread.ManagedThreadId));
        }

        void DlgtFunc_WhatsInBox<TGift, TReletive>(TGift gift, TBox box, TReletive fromWhom, Baby baby)
        {
            System.Diagnostics.Trace.WriteLine(string.Format("[PID:{2}] {0}.{1}() begin", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Thread.CurrentThread.ManagedThreadId));
            GiftBase g = gift as GiftBase;
            Reletive r = fromWhom as Reletive;
            box.GiftIntoBox(g, r);
            //System.Diagnostics.Trace.WriteLine(string.Format("[PID:{2}] {0}.{1}() gift has been packaged in box", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Thread.CurrentThread.ManagedThreadId));
            r.GiveToBady(box, baby);
            //System.Diagnostics.Trace.WriteLine(string.Format("[PID:{2}] {0}.{1}() box has been given to baby", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Thread.CurrentThread.ManagedThreadId));
            baby.OpenBox(box);
            //System.Diagnostics.Trace.WriteLine(string.Format("[PID:{2}] {0}.{1}() box has been openned.", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Thread.CurrentThread.ManagedThreadId));
            baby.BabyResponse();
            //System.Diagnostics.Trace.WriteLine(string.Format("[PID:{2}] {0}.{1}() baby responsed.", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Thread.CurrentThread.ManagedThreadId));
            System.Diagnostics.Trace.WriteLine(string.Format("[PID:{2}] {0}.{1}() end", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Thread.CurrentThread.ManagedThreadId));
        }

        void Acb_WhatsInBox(IAsyncResult iar)
        {
            System.Diagnostics.Trace.WriteLine(string.Format("[PID:{2}] {0}.{1}() begin", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Thread.CurrentThread.ManagedThreadId));
            if (_boxes.Count > CurrentIdx)
            {
                _ares[CurrentIdx].Set();
                CurrentIdx++;
                if (_boxes.Count > CurrentIdx)
                {
                    GiftBase gift = null;
                    Reletive fromWhom = null;
                    BuyGift(out gift, out fromWhom);
                    var _dlgtWhatsInBox = new DefDlgt_WhatsInBox<GiftBase, Reletive>(DlgtFunc_WhatsInBox);
                    //System.Diagnostics.Trace.WriteLine(string.Format("[PID:{2}] {0}.{1}() gift has been bought", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Thread.CurrentThread.ManagedThreadId));
                    _dlgtWhatsInBox.BeginInvoke(gift, _boxes[CurrentIdx], fromWhom, baby, Acb_WhatsInBox, _dlgtWhatsInBox);
                    //System.Diagnostics.Trace.WriteLine(string.Format("[PID:{2}] {0}.{1}() see what's in box...", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Thread.CurrentThread.ManagedThreadId));
                }
            }
            (iar.AsyncState as DefDlgt_WhatsInBox<GiftBase, Reletive>).EndInvoke(iar);
            System.Diagnostics.Trace.WriteLine(string.Format("[PID:{2}] {0}.{1}() end", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Thread.CurrentThread.ManagedThreadId));
        }
    }

    class TBox
    {
        public int Code { get; protected set; }

        public TBox(int i)
        {
            Code = i;
        }

        public override string ToString()
        {
            return string.Format("Box-{0}", Code);
        }
        public string GiftIntoBox(GiftBase gift, Reletive fromWhom)
        {
            string rslt = string.Format("A/An {0} from {1} has been put in the {2}.", gift, fromWhom, ToString());
            System.Diagnostics.Trace.WriteLine(string.Format("[PID:{3}] {0}.{1}(), {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, rslt, Thread.CurrentThread.ManagedThreadId));
            return rslt;
        }
    }

    #region Reletives
    class Reletive
    {
        public string FullName { get; protected set; }
        protected Reletive(string name)
        {
            FullName = name;
        }
        public override string ToString()
        {
            return FullName;
        }
        virtual public string GiveToBady(TBox gift, Baby baby)
        {
            string rslt = string.Format("A box of {0} has been given to {1}.", gift, baby);
            System.Diagnostics.Trace.WriteLine(string.Format("[PID:{3}] {0}.{1}(), {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, rslt, Thread.CurrentThread.ManagedThreadId));
            return rslt;
        }
    }

    class Baby : Reletive
    {
        public Baby(string name) : base(name) { }
        public override string GiveToBady(TBox gift, Baby baby)
        {
            string rslt = string.Format("\"!Oops, baby cannot give gift to himself.\" says {0}", FullName);
            System.Diagnostics.Trace.WriteLine(string.Format("[PID:{3}] {0}.{1}(), {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, rslt, Thread.CurrentThread.ManagedThreadId));
            return rslt;
        }
        public string OpenBox(TBox box)
        {
            string rslt = string.Format("{0} has opened {1}.", FullName, box);
            System.Diagnostics.Trace.WriteLine(string.Format("[PID:{3}] {0}.{1}(), {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, rslt, Thread.CurrentThread.ManagedThreadId));
            return rslt;
        }
        public string BabyResponse()
        {
            Random rand = new Random(1);
            int seed = rand.Next(2);
            string rslt = (1 == seed) ? string.Format("{0} give a big laught.", FullName) : string.Format("{0} gives a big hug.", FullName);
            System.Diagnostics.Trace.WriteLine(string.Format("[PID:{3}] {0}.{1}(), {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, rslt, Thread.CurrentThread.ManagedThreadId));
            return rslt;
        }
    }

    class Mother : Reletive
    {
        public Mother(string name) : base(name)
        { }
    }

    class Father : Reletive
    {
        public Father(string name) : base(name)
        { }
    }

    class Grandpa : Reletive
    {
        public Grandpa(string name) : base(name)
        { }
    }

    class Grandma : Reletive
    {
        public Grandma(string name) : base(name)
        { }
    }
    #endregion

    #region Gift
    abstract class GiftBase
    {
        protected GiftBase(string price, string fromWhom)
        {
            Price = price;
            FromWhom = fromWhom;
        }

        abstract public string Name { get; }
        public string Price { get; set; }
        public string FromWhom { get; set; }
        public override string ToString()
        {
            return string.Format("{0} buy a/an {1} which cost ${2}", FromWhom, Name, Price);
        }
    }

    class Gift_Book : GiftBase
    {
        public Gift_Book(string price, string fromWhom) : base(price, fromWhom) { }
        public override string Name { get { return "Harry Potter I"; } }
    }

    class Gift_Toy : GiftBase
    {
        public Gift_Toy(string price, string fromWhom) : base(price, fromWhom) { }
        public override string Name { get { return "Dinassor"; } }
    }

    class Gift_Bike : GiftBase
    {
        public Gift_Bike(string price, string fromWhom) : base(price, fromWhom) { }
        public override string Name { get { return "BMW bicycle"; } }
    }
    #endregion
}
