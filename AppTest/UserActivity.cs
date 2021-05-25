using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using AppTest.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace AppTest
{
    [Activity(Label = "List of Users", Theme = "@style/AppTheme")]
    public class UserActivity : AppCompatActivity
    {
        List<string> items = null;
        private ListView listView;
        static List<User> users = new List<User>();

        // GET: List of users
        public void GetUsersFromJSON()
        {
            string url = "https://randomuser.me/api/";

            users.Clear();

            WebClient w = new WebClient();

            var json_data = w.DownloadString(url);

            dynamic data = null;
            data = JObject.Parse(json_data);

            for (int i = 0; i < data.results.Count; i++)
            {
                User user = new User();

                DateTime dateN = Convert.ToDateTime(data.results[i].dob.date);

                user.UserID = i + 1;
                user.Abbreviation = data.results[i].name.title;
                user.FisrtName = data.results[i].name.first;
                user.LastName = data.results[i].name.last;
                user.Age = data.results[i].dob.age;
                user.DateOfBirth = dateN.Date.ToLongDateString();
                user.Gender = data.results[i].gender;
                user.Email = data.results[i].email;
                user.Phone = data.results[i].phone;
                user.Location = (data.results[i].location.city + "," + data.results[i].location.country);
                user.Picture = data.results[i].picture.medium;
                user.PictureLg = data.results[i].picture.large;

                users.Add(user);
            }
        }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_user);

            listView = FindViewById<ListView>(Resource.Id.userList);

            GetUsersFromJSON();

            items = new List<string>();

            foreach (var x in users)
            {
                items.Add(x.FisrtName + " " + x.LastName);
            }

            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, items);

            listView.Adapter = adapter;

            listView.ItemSelected += ListView_ItemSelected;

        }

        private void ListView_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
        }
    }
}