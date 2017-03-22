using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using RandomPasswordGenerator.Models;
using System.Collections.Generic;

namespace RandomPasswordGenerator
{
    [Activity(Label = "@string/application_name", MainLauncher = true, Icon = "@drawable/ic_launcher", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : Activity, SeekBar.IOnSeekBarChangeListener
    {

        List<Bookmark> bookmarks = new List<Bookmark>()
        {
            new Bookmark { Source = "Example Source", Password = "ABC123" },
        };

        // List View Adapter
        BookmarksViewAdapter viewAdapter;

        // External variable declarations
        Button copyBtn;
        Button saveBtn;
        Button generateBtn;

        SeekBar lengthSeekBar;
        TextView valueSeekBar;

        CheckBox checkboxUpperCase;
        CheckBox checkboxLowerCase;
        CheckBox checkboxDigits;
        CheckBox checkboxSpecialCharacters;

        EditText mApplicationNameInput;
        EditText displayPassword;

        // Internal variable declarations
        const string UPPER_CASE_STRING = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string LOWER_CASE_STRING = "abcdefghijklmnopqrstuvwxyz";
        const string DIGITS_STRING = "01234567890123456789012345";
        const string SPECIAL_CHARACTER_STRING = "!#%&()*+,-./~:;<=>?@[\\]^_{|}";

        static int passwordLength = 12;

        static bool allowUpperCase = true;
        static bool allowLowerCase = true;
        static bool allowDigits = true;
        static bool allowSpecialCharacters = true;

        static string password;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Layout set to HomeScreen
            SetContentView(Resource.Layout.HomeLayout);

            // Button declarations
            copyBtn = FindViewById<Button>(Resource.Id.btnCopy);
            copyBtn.Click += BtnCopy_Click;
            saveBtn = FindViewById<Button>(Resource.Id.btnSave);
            saveBtn.Click += BtnSave_Click;
            generateBtn = FindViewById<Button>(Resource.Id.btnGenerate);
            generateBtn.Click += GenerateBtn_Click;

            // Toolbar declaration
            var toolbarHome = FindViewById<Toolbar>(Resource.Id.toolbarHome);
            SetActionBar(toolbarHome);
            ActionBar.Title = "Password Generator";

            // Seekbar & value declaration
            lengthSeekBar = FindViewById<SeekBar>(Resource.Id.seekBarLength);
            valueSeekBar = FindViewById<TextView>(Resource.Id.seekBarValue);
            lengthSeekBar.SetOnSeekBarChangeListener(this);

            // Checkbox declarations
            checkboxUpperCase = FindViewById<CheckBox>(Resource.Id.upperCaseCheckbox);
            checkboxUpperCase.Click += CheckboxUpperCase_Click;
            checkboxLowerCase = FindViewById<CheckBox>(Resource.Id.lowerCaseCheckbox);
            checkboxLowerCase.Click += CheckboxLowerCase_Click;
            checkboxDigits = FindViewById<CheckBox>(Resource.Id.digitsCheckbox);
            checkboxDigits.Click += CheckboxDigits_Click;
            checkboxSpecialCharacters = FindViewById<CheckBox>(Resource.Id.specialCharactersCheckbox);
            checkboxSpecialCharacters.Click += CheckboxSpecialCharacters_Click;

            // Edittext declaration
            displayPassword = FindViewById<EditText>(Resource.Id.generatedPassword);

            viewAdapter = new BookmarksViewAdapter(this, bookmarks);
        }

        private void CheckboxSpecialCharacters_Click(object sender, EventArgs e)
        {
            if (checkboxSpecialCharacters.Checked) { allowSpecialCharacters = true; }
            else
                allowSpecialCharacters = false;
        }

        private void CheckboxDigits_Click(object sender, EventArgs e)
        {
            if (checkboxDigits.Checked) { allowDigits = true; }
            else
                allowDigits = false;
        }

        private void CheckboxLowerCase_Click(object sender, EventArgs e)
        {
            if (checkboxLowerCase.Checked) { allowLowerCase = true; }
            else
                allowLowerCase = false;
        }

        private void CheckboxUpperCase_Click(object sender, EventArgs e)
        {
            if (checkboxUpperCase.Checked) { allowUpperCase = true; }
            else
                allowUpperCase = false;
        }

        // BtnGenerate
        private void GenerateBtn_Click(object sender, EventArgs e)
        {

            char[] passwordChar = new char[passwordLength];
            char repeatLetter;
            string allowedLetterString = DetermineAllowedLetterString();
            var allowedLetterStringChar = allowedLetterString.ToCharArray();
            var randomLetter = new Random();

            if (allowedLetterString == "")
            {
                DisplayToastText("Enable at least one checkbox.");
            }
            else
            {
                repeatLetter = allowedLetterStringChar[randomLetter.Next(allowedLetterString.Length)];
                passwordChar[0] = repeatLetter;

                for (int i = 1; i < passwordLength; i++)
                {
                    do
                    {
                        repeatLetter = allowedLetterStringChar[randomLetter.Next(allowedLetterString.Length)];
                    } while (repeatLetter == passwordChar[i - 1]);

                    passwordChar[i] = repeatLetter;
                    // passwordChar[i] = allowedLetterStringChar[randomLetter.Next(allowedLetterString.Length)];
                }
            }

            password = new String(passwordChar);
            if (password.Length > 0) { displayPassword.Text = password; }
            if (allowUpperCase == false && allowLowerCase == false && allowDigits == false && allowSpecialCharacters == false)
            {
                saveBtn.Enabled = false;
            }
            else
            {
                saveBtn.Enabled = true;
            }
        }

        // Med denna kod slapp jag skriva 16 if-satser
        private string DetermineAllowedLetterString()
        {
            string completeLetterString = "";
            bool[] letterBool = new bool[] { allowUpperCase, allowLowerCase, allowDigits, allowSpecialCharacters };
            string[] letterString = new string[] { UPPER_CASE_STRING, LOWER_CASE_STRING, DIGITS_STRING, SPECIAL_CHARACTER_STRING };

            for (int i = 0; i < letterBool.Length; i++)
            {
                if (letterBool[i] == true)
                    completeLetterString += letterString[i];
            }

            return completeLetterString;
        }

        // btnCopy
        private void BtnCopy_Click(object sender, EventArgs e)
        {
            ClipboardManager clipboard = (ClipboardManager)GetSystemService(ClipboardService);
            ClipData clip = ClipData.NewPlainText("Copy Password", password);
            clipboard.PrimaryClip = clip;
            DisplayToastText("Added to clipboard.");
        }

        // btnSave
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (displayPassword.Text.Length < 1)
            {
                DisplayToastText("Generate a password first.");
            }
            else
            {
                LayoutInflater layoutInflaterSaveDialog = LayoutInflater.From(this);
                View homeView = layoutInflaterSaveDialog.Inflate(Resource.Layout.dialog_save, null);
                AlertDialog.Builder alertDialogBuilderUserInput = new AlertDialog.Builder(this);
                alertDialogBuilderUserInput.SetView(homeView);

                mApplicationNameInput = homeView.FindViewById<EditText>(Resource.Id.application_name_input);

                alertDialogBuilderUserInput
                    .SetCancelable(false)
                    .SetPositiveButton("Save Password", delegate
                    {
                        // bookmarksList.add(mApplicationNameInput);  <-->  Password Name
                        // bookmarksList.add(password);  <-->  Password 
                        DisplayToastText("Password saved.");
                        saveBtn.Enabled = false;
                        bookmarks.Add(new Bookmark() { Source = mApplicationNameInput.Text, Password = password });                      
                    })
                    .SetNegativeButton("Cancel", delegate
                    {
                        alertDialogBuilderUserInput.Dispose();
                    });

                AlertDialog alertDialogAndroid = alertDialogBuilderUserInput.Create();
                alertDialogAndroid.Show();
            }
        }

        // Enable menu/action items on the toolbar
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_toolbar_home, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        // Handle click events for said menu/action items 
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                // Bookmarks action
                case Resource.Id.menu_bookmark:
                    // Go to bookmarksActivity (page) when pressing the bookmarks menu icon
                    Intent intentBookmark = new Intent(this, typeof(BookmarksActivity));
                    this.StartActivity(intentBookmark);
                    return true;
                // About action
                case Resource.Id.menu_about:
                    LayoutInflater layoutInflaterAboutDialog = LayoutInflater.From(this);
                    View homeView_2 = layoutInflaterAboutDialog.Inflate(Resource.Layout.dialog_about, null);
                    AlertDialog.Builder alertDialogAbout = new AlertDialog.Builder(this);
                    alertDialogAbout.SetView(homeView_2);
                    alertDialogAbout
                        .SetNegativeButton("Cancel", delegate
                        {
                            alertDialogAbout.Dispose();
                        });
                    AlertDialog alertDialogAndroid_1 = alertDialogAbout.Create();
                    alertDialogAndroid_1.Show();
                    return true;
                // Change log action
                case Resource.Id.menu_change_log:
                    LayoutInflater layoutInflaterChangeLogDialog = LayoutInflater.From(this);
                    View homeView_3 = layoutInflaterChangeLogDialog.Inflate(Resource.Layout.dialog_change_log, null);
                    AlertDialog.Builder alertDialogChangeLog = new AlertDialog.Builder(this);
                    alertDialogChangeLog.SetView(homeView_3);
                    alertDialogChangeLog
                        .SetNegativeButton("Cancel", delegate
                        {
                            alertDialogChangeLog.Dispose();
                        });
                    AlertDialog alertDialogAndroid_2 = alertDialogChangeLog.Create();
                    alertDialogAndroid_2.Show();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            if (fromUser)
            {
                valueSeekBar.Text = string.Format("{0}", seekBar.Progress + 1);
            }
            passwordLength = seekBar.Progress + 1;
        }

        public void OnStartTrackingTouch(SeekBar seekBar)
        {
            // 
        }

        public void OnStopTrackingTouch(SeekBar seekBar)
        {
            //
        }

        private void DisplayToastText(string toastText)
        {
            Toast.MakeText(this, toastText, ToastLength.Short).Show();
        }

        
    }
}

