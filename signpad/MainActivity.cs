using Android.App;
using Android.Widget;
using Android.OS;
using SignaturePad;
using Android.Graphics;
using Java.IO;
using System.IO;
using System;

namespace signpad
{
    [Activity(Label = "Signature pad", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        readonly string ContentDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            var signaturePad = FindViewById<SignaturePadView>(Resource.Id.signatureView);
            var btnSave = FindViewById<Button>(Resource.Id.btnSave);

            signaturePad.Caption.Text = "Authorised signature";
            signaturePad.Caption.SetTypeface(Typeface.Serif, TypefaceStyle.BoldItalic);
            signaturePad.Caption.SetTextSize(global::Android.Util.ComplexUnitType.Sp, 16f);
            signaturePad.SignaturePrompt.SetTypeface(Typeface.SansSerif, TypefaceStyle.Normal);
            signaturePad.SignaturePrompt.SetTextSize(global::Android.Util.ComplexUnitType.Sp, 32f);
            signaturePad.BackgroundColor = Color.Wheat;
            signaturePad.StrokeColor = Color.Black;

            signaturePad.BackgroundImageView.Alpha = 16f;
            signaturePad.BackgroundImageView.SetAdjustViewBounds(true);
            var layout = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.FillParent, RelativeLayout.LayoutParams.FillParent);
            layout.AddRule(LayoutRules.CenterInParent);
            layout.SetMargins(20, 20, 20, 20);
            signaturePad.BackgroundImageView.LayoutParameters = layout;

            var caption = signaturePad.Caption;
            caption.SetPadding(caption.PaddingLeft, 1, caption.PaddingRight, 25);

            btnSave.Click += delegate
            {
                if (signaturePad.IsBlank)
                {
                    Toast.MakeText(this, "The signature is blank", ToastLength.Long).Show();
                    return;
                }

                var image = signaturePad.GetImage();
                var dtn = DateTime.Now.ToString().Replace(' ', '*').Replace('/', '_');
                var filename = System.IO.Path.Combine(ContentDirectory, string.Format("signature-{0}.jpg", dtn));
                if (!System.IO.File.Exists(filename))
                    System.IO.File.Create(filename);

                using (var ms = new MemoryStream())
                {
                    var imgJpeg = image.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                    var buffer = ms.GetBuffer();
                    using (var output = new FileOutputStream(filename))
                        output.Write(buffer);
                }
            };
        }
    }
}


