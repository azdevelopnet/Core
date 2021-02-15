#if __ANDROID__
using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Renderscripts;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.App;
using Plugin.CurrentActivity;
using Xamarin.Forms.Core;
using static Android.Views.View;
using DroidRect = Android.Graphics.Rect;
using DroidView = Android.Views.View;
using Renderscripts = Android.Renderscripts;
using Resource = Android.Resource;

[assembly: Xamarin.Forms.Dependency(typeof(BlurOverlay))]
namespace Xamarin.Forms.Core
{
    public class BlurOverlay : IBlurOverlay
    {
        public Task BlurAsync()
        {
            var source = new TaskCompletionSource<Task>();

            CreateDialog();
            SetupDialog(source);
            ShowDialog();

            return source.Task;
        }

        private void CreateDialog()
        {
            _blurredDialog = new BlurFragmentDialog();
        }

        private void SetupDialog(TaskCompletionSource<Task> source)
        {
            _blurredDialog.BlurCompletionAction = () =>
            {
                source.SetResult(Task.FromResult(true));
            };
        }

        private void ShowDialog()
        {
            var activity = (AppCompatActivity)CrossCurrentActivity.Current.Activity;
            _blurredDialog.Show(activity.SupportFragmentManager, nameof(BlurFragmentDialog));
        }

        /// <summary>
        /// Unblurs entire screen
        /// </summary>
        public void Unblur()
        {
            if (_blurredDialog == null)
            {
                return;
            }

            _blurredDialog.Dismiss();
            _blurredDialog.Dispose();
            _blurredDialog = null;
        }

        private BlurFragmentDialog _blurredDialog;
    }

    public class BlurFragmentDialog : AndroidX.Fragment.App.DialogFragment
    {
        public Action BlurCompletionAction { get; set; }

        public override void OnStart()
        {
            base.OnStart();

            var blurredScreenBitmap = BlurUtility.GetBlurredScreen(CrossCurrentActivity.Current.Activity);
            var draw = new BitmapDrawable(Resources, blurredScreenBitmap);
            Dialog.Window.SetBackgroundDrawable(draw);

            BlurCompletionAction?.Invoke();
        }
    }
    public static class BlurUtility
    {
        public static Bitmap GetBlurredScreen(Activity activity, Int32 blurredRadius = 20)
        {
            var map = TakeScreenShot(activity);
            var blurred = BlurBitmap(map, blurredRadius);

            return blurred;
        }

        //private static Bitmap TakeScreenShot(Activity activity)
        //{
        //    var view = activity.Window.DecorView;
        //    view.DrawingCacheEnabled = true;
        //    view.BuildDrawingCache();
        //    Bitmap b1 = view.DrawingCache;
        //    DroidRect frame = new DroidRect();
        //    activity.Window.DecorView.GetWindowVisibleDisplayFrame(frame);
        //    int statusBarHeight = frame.Top;

        //    DisplayMetrics displaymetrics = new DisplayMetrics();
        //    activity.WindowManager.DefaultDisplay.GetMetrics(displaymetrics);

        //    int width = displaymetrics.WidthPixels;
        //    int height = displaymetrics.HeightPixels;

        //    Bitmap b = Bitmap.CreateBitmap(b1, 0, statusBarHeight, width, height - statusBarHeight);
           
        //    view.DestroyDrawingCache();
        //    return b;
        //}

        public static Bitmap TakeScreenShot(Activity activity)
        {
            var view = activity.Window.DecorView;
            Bitmap bitmap = Bitmap.CreateBitmap(view.Width, view.Height, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(bitmap);
            view.Draw(canvas);
            return bitmap;
        }

        private static Bitmap BlurBitmap(Bitmap sentBitmap, int radius)
        {
            Bitmap bitmap = sentBitmap.Copy(sentBitmap.GetConfig(), true);

            if (radius < 1)
            {
                return (null);
            }

            int w = bitmap.Width;
            int h = bitmap.Height;

            int[] pix = new int[w * h];

            bitmap.GetPixels(pix, 0, w, 0, 0, w, h);

            int wm = w - 1;
            int hm = h - 1;
            int wh = w * h;
            int div = radius + radius + 1;

            int[] r = new int[wh];
            int[] g = new int[wh];
            int[] b = new int[wh];
            int rsum, gsum, bsum, x, y, i, p, yp, yi, yw;
            int[] vmin = new int[Math.Max(w, h)];

            int divsum = (div + 1) >> 1;
            divsum *= divsum;
            int[] dv = new int[256 * divsum];
            for (i = 0; i < 256 * divsum; i++)
            {
                dv[i] = (i / divsum);
            }

            yw = yi = 0;

            int[][] stack = new int[div][];

            for (int item = 0; item < div; item++)
            {
                stack[item] = new int[3];
            }

            int stackpointer;
            int stackstart;
            int[] sir;
            int rbs;
            int r1 = radius + 1;
            int routsum, goutsum, boutsum;
            int rinsum, ginsum, binsum;

            for (y = 0; y < h; y++)
            {
                rinsum = ginsum = binsum = routsum = goutsum = boutsum = rsum = gsum = bsum = 0;
                for (i = -radius; i <= radius; i++)
                {
                    p = pix[yi + Math.Min(wm, Math.Max(i, 0))];
                    sir = stack[i + radius];
                    sir[0] = (p & 0xff0000) >> 16;
                    sir[1] = (p & 0x00ff00) >> 8;
                    sir[2] = (p & 0x0000ff);
                    rbs = r1 - Math.Abs(i);
                    rsum += sir[0] * rbs;
                    gsum += sir[1] * rbs;
                    bsum += sir[2] * rbs;
                    if (i > 0)
                    {
                        rinsum += sir[0];
                        ginsum += sir[1];
                        binsum += sir[2];
                    }
                    else
                    {
                        routsum += sir[0];
                        goutsum += sir[1];
                        boutsum += sir[2];
                    }
                }
                stackpointer = radius;

                for (x = 0; x < w; x++)
                {

                    r[yi] = dv[rsum];
                    g[yi] = dv[gsum];
                    b[yi] = dv[bsum];

                    rsum -= routsum;
                    gsum -= goutsum;
                    bsum -= boutsum;

                    stackstart = stackpointer - radius + div;
                    sir = stack[stackstart % div];

                    routsum -= sir[0];
                    goutsum -= sir[1];
                    boutsum -= sir[2];

                    if (y == 0)
                    {
                        vmin[x] = Math.Min(x + radius + 1, wm);
                    }
                    p = pix[yw + vmin[x]];

                    sir[0] = (p & 0xff0000) >> 16;
                    sir[1] = (p & 0x00ff00) >> 8;
                    sir[2] = (p & 0x0000ff);

                    rinsum += sir[0];
                    ginsum += sir[1];
                    binsum += sir[2];

                    rsum += rinsum;
                    gsum += ginsum;
                    bsum += binsum;

                    stackpointer = (stackpointer + 1) % div;
                    sir = stack[(stackpointer) % div];

                    routsum += sir[0];
                    goutsum += sir[1];
                    boutsum += sir[2];

                    rinsum -= sir[0];
                    ginsum -= sir[1];
                    binsum -= sir[2];

                    yi++;
                }
                yw += w;
            }
            for (x = 0; x < w; x++)
            {
                rinsum = ginsum = binsum = routsum = goutsum = boutsum = rsum = gsum = bsum = 0;
                yp = -radius * w;
                for (i = -radius; i <= radius; i++)
                {
                    yi = Math.Max(0, yp) + x;

                    sir = stack[i + radius];

                    sir[0] = r[yi];
                    sir[1] = g[yi];
                    sir[2] = b[yi];

                    rbs = r1 - Math.Abs(i);

                    rsum += r[yi] * rbs;
                    gsum += g[yi] * rbs;
                    bsum += b[yi] * rbs;

                    if (i > 0)
                    {
                        rinsum += sir[0];
                        ginsum += sir[1];
                        binsum += sir[2];
                    }
                    else
                    {
                        routsum += sir[0];
                        goutsum += sir[1];
                        boutsum += sir[2];
                    }

                    if (i < hm)
                    {
                        yp += w;
                    }
                }
                yi = x;
                stackpointer = radius;
                for (y = 0; y < h; y++)
                {
                    pix[yi] = (int)((0xff000000 & pix[yi]) | (dv[rsum] << 16) | (dv[gsum] << 8) | dv[bsum]);

                    rsum -= routsum;
                    gsum -= goutsum;
                    bsum -= boutsum;

                    stackstart = stackpointer - radius + div;
                    sir = stack[stackstart % div];

                    routsum -= sir[0];
                    goutsum -= sir[1];
                    boutsum -= sir[2];

                    if (x == 0)
                    {
                        vmin[y] = Math.Min(y + r1, hm) * w;
                    }
                    p = x + vmin[y];

                    sir[0] = r[p];
                    sir[1] = g[p];
                    sir[2] = b[p];

                    rinsum += sir[0];
                    ginsum += sir[1];
                    binsum += sir[2];

                    rsum += rinsum;
                    gsum += ginsum;
                    bsum += binsum;

                    stackpointer = (stackpointer + 1) % div;
                    sir = stack[stackpointer];

                    routsum += sir[0];
                    goutsum += sir[1];
                    boutsum += sir[2];

                    rinsum -= sir[0];
                    ginsum -= sir[1];
                    binsum -= sir[2];

                    yi += w;
                }
            }

            bitmap.SetPixels(pix, 0, w, 0, 0, w, h);

            return (bitmap);
        }
    }
}
#endif
