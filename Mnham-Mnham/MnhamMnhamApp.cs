using System;
using Android.App;
using System.Runtime.CompilerServices;
using Android.Runtime;

namespace Mnham_Mnham
{
    [Application]
    public class MnhamMnhamApp : Application
	{
		private UtilitarioApiGoogle utilitarioApiGoogle;
		private static MnhamMnhamApp instancia;

	    public MnhamMnhamApp(IntPtr handle, JniHandleOwnership transfer)
	        : base(handle,transfer)
	    {
	        instancia = this;
	        utilitarioApiGoogle = new UtilitarioApiGoogle(instancia);
        }

        /*public override void OnCreate()
		{
			base.OnCreate();

			instancia = this;
			utilitarioApiGoogle = new UtilitarioApiGoogle(instancia);
		}*/

		[MethodImpl(MethodImplOptions.Synchronized)]
		public static MnhamMnhamApp ObterInstancia()
		{
			return instancia;
		}

		public UtilitarioApiGoogle ObterInstanciaUtilitarioApiGoogle()
		{
			return utilitarioApiGoogle;
		}

		public static UtilitarioApiGoogle ObterUtilitarioApiGoogle()
		{
		    MnhamMnhamApp app = ObterInstancia();

			return app.ObterInstanciaUtilitarioApiGoogle();
		}
	}
}
