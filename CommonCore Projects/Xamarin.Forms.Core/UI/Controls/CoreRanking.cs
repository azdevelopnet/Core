using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace Xamarin.Forms.Core
{
    [DesignTimeVisible(true)]
    public class CoreRanking : StackLayout, IDisposable
	{
		private List<Grid> starList;
		public List<Grid> StarList
		{
			get { return starList ?? (starList = new List<Grid>()); }
			set { starList = value; }
		}

		public static readonly BindableProperty CountProperty =
	                BindableProperty.Create(propertyName: "Count",
				    returnType: typeof(int),
				    declaringType: typeof(CoreRanking),
				    defaultValue: 0,
				    propertyChanged: OnCountPropertyChanged);
        

		public int Count
		{
			get { return (int)GetValue(CountProperty); }
			set
			{
				SetValue(CountProperty, value);
			}
		}

		public static readonly BindableProperty UnSelectedImageProperty =
				BindableProperty.Create("UnSelectedImage",
										typeof(string),
										typeof(CoreRanking),
										null);

		public string UnSelectedImage
		{
			get { return (string)GetValue(UnSelectedImageProperty); }
			set { SetValue(UnSelectedImageProperty, value); }
		}


		public static readonly BindableProperty SelectedImageProperty =
				BindableProperty.Create("SelectedImage",
										typeof(string),
										typeof(CoreRanking),
										null);

		public string SelectedImage
		{
			get { return (string)GetValue(SelectedImageProperty); }
			set { SetValue(SelectedImageProperty, value); }
		}

		public static readonly BindableProperty SelectedRankProperty =
			BindableProperty.Create("SelectedRank",
									typeof(int),
									typeof(CoreRanking),
									0);

		public int SelectedRank
		{
			get { return (int)this.GetValue(SelectedRankProperty); }
			set
			{

				this.SetValue(SelectedRankProperty, value);
			}
		}

		public CoreRanking()
		{
			this.HeightRequest = 44;
			this.Orientation = StackOrientation.Horizontal;
		}

		public void InitControl()
		{
			foreach (var star in StarList)
			{
				var behavior = (StarBehavior)star.Behaviors[0];
				behavior.PropertyChanged -= RatingchangedEvent;
				star.Behaviors.Remove(behavior);
				Children.Remove(star);
			}

			for (int x = 0; x < this.Count; x++)
			{
				var behavior = new StarBehavior() { GroupName = "starGrouping" };
				behavior.Index = (x + 1);
				var gd = new Grid();
				var unSelectedImg = new Image();
				unSelectedImg.SetBinding(Image.SourceProperty, new Binding(source: this, path: "UnSelectedImage"));
				var selectedImg = new Image();
				selectedImg.SetBinding(Image.SourceProperty, new Binding(source: this, path: "SelectedImage"));
				selectedImg.SetBinding(Image.IsVisibleProperty, new Binding(source: behavior, path: "IsStarred"));
				gd.Children.Add(unSelectedImg, 0, 0);
				gd.Children.Add(selectedImg, 0, 0);

				if (x == (Count - 1))
					behavior.PropertyChanged += RatingchangedEvent;

				gd.Behaviors.Add(behavior);
				Children.Add(gd);
			}
		}

		private static void OnCountPropertyChanged(BindableObject bindable, object value, object newValue)
		{
			((CoreRanking)bindable).InitControl();
		}

		private void RatingchangedEvent(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == "Rating")
			{
				var behavior = (StarBehavior)sender;
				SelectedRank = behavior.Rating;
			}
		}
		~CoreRanking()
		{
            var cnt = Count > 0 ? Count : 0;
            var behavior = (StarBehavior)StarList[cnt - 1].Behaviors[0];
			behavior.PropertyChanged -= RatingchangedEvent;
		}
		public void Dispose()
		{
            var cnt = Count > 0 ? Count : 0;
			var behavior = (StarBehavior)StarList[cnt - 1].Behaviors[0];
			behavior.PropertyChanged -= RatingchangedEvent;
		}
	}

	public class StarBehavior : Behavior<View>
	{
		public int Index { get; set; }
		TapGestureRecognizer tapRecognizer;
		static List<StarBehavior> defaultBehaviors = new List<StarBehavior>();
		static Dictionary<string, List<StarBehavior>> starGroups = new Dictionary<string, List<StarBehavior>>();

		public static readonly BindableProperty GroupNameProperty =
			BindableProperty.Create("GroupName",
									typeof(string),
									typeof(StarBehavior),
									null,
									propertyChanged: OnGroupNameChanged);

		public string GroupName
		{
			set { SetValue(GroupNameProperty, value); }
			get { return (string)GetValue(GroupNameProperty); }
		}

		public static readonly BindableProperty RatingProperty =
		   BindableProperty.Create("Rating",
								   typeof(int),
								   typeof(StarBehavior), default(int));

		public int Rating
		{
			set { SetValue(RatingProperty, value); }
			get { return (int)GetValue(RatingProperty); }
		}

		static void OnGroupNameChanged(BindableObject bindable, object oldValue, object newValue)
		{
			StarBehavior behavior = (StarBehavior)bindable;
			string oldGroupName = (string)oldValue;
			string newGroupName = (string)newValue;

			// Remove existing behavior from Group
			if (String.IsNullOrEmpty(oldGroupName))
			{
				defaultBehaviors.Remove(behavior);
			}
			else
			{
				List<StarBehavior> behaviors = starGroups[oldGroupName];
				behaviors.Remove(behavior);

				if (behaviors.Count == 0)
				{
					starGroups.Remove(oldGroupName);
				}
			}

			// Add New Behavior to the group
			if (String.IsNullOrEmpty(newGroupName))
			{
				defaultBehaviors.Add(behavior);
			}
			else
			{
				List<StarBehavior> behaviors = null;

				if (starGroups.ContainsKey(newGroupName))
				{
					behaviors = starGroups[newGroupName];
				}
				else
				{
					behaviors = new List<StarBehavior>();
					starGroups.Add(newGroupName, behaviors);
				}

				behaviors.Add(behavior);
			}

		}


		public static readonly BindableProperty IsStarredProperty =
			BindableProperty.Create("IsStarred",
									typeof(bool),
									typeof(StarBehavior),
									false,
									propertyChanged: OnIsStarredChanged);

		public bool IsStarred
		{
			set { SetValue(IsStarredProperty, value); }
			get { return (bool)GetValue(IsStarredProperty); }
		}

		static void OnIsStarredChanged(BindableObject bindable, object oldValue, object newValue)
		{
			StarBehavior behavior = (StarBehavior)bindable;

			if ((bool)newValue)
			{
				string groupName = behavior.GroupName;
				List<StarBehavior> behaviors = null;

				if (string.IsNullOrEmpty(groupName))
				{
					behaviors = defaultBehaviors;
				}
				else
				{
					behaviors = starGroups[groupName];
				}

				bool itemReached = false;

				foreach (var item in behaviors)
				{
					if (item != behavior && !itemReached)
					{
						item.IsStarred = true;
					}
					if (item == behavior)
					{
						itemReached = true;
						item.IsStarred = true;
					}
					if (item != behavior && itemReached)
						item.IsStarred = false;

					item.Rating = behavior.Index;

				}

			}


		}

		public StarBehavior()
		{
			defaultBehaviors.Add(this);
		}

		protected override void OnAttachedTo(View view)
		{
			tapRecognizer = new TapGestureRecognizer();
			tapRecognizer.Tapped += OnTapRecognizerTapped;
			view.GestureRecognizers.Add(tapRecognizer);
		}

		protected override void OnDetachingFrom(View view)
		{
			view.GestureRecognizers.Remove(tapRecognizer);
			tapRecognizer.Tapped -= OnTapRecognizerTapped;
		}

		void OnTapRecognizerTapped(object sender, EventArgs args)
		{
			//HACK: PropertyChange does not fire, if the value is not changed :-(
			IsStarred = false;
			IsStarred = true;
		}
	}
}

