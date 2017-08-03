using ClassManager.Models;
using ClassManager.Networks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManager.ViewModels
{
    public class PersonViewModel: BaseViewModel
    {
        APIService api = new APIService();

        public string Total {
            get {
                if (Groups != null)
                { 
                    return Groups.Count().ToString();
                }
                else
                {
                    return "None";
                }
            }
        }
        /// <summary>
        /// 学生分组列表
        /// </summary>
        private ObservableCollection<PersonGroup> _groups;
        public ObservableCollection<PersonGroup> Groups {
            get {
                return this._groups;
            }
            set {
                this._groups = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 学生列表
        /// </summary>
        private List<Person> _persons;
        public List<Person> Persons {
            get {
                return this._persons;
            }
            set {
                this._persons = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 被选中的学生
        /// </summary>
        private Person _person_on_display;
        public Person personOnDisplay {
            get {
                return _person_on_display;
            }
            set {
                this._person_on_display = value;
                OnPropertyChanged();
            }
        }

        public async Task Init()
        {
            await RefreshPersons();
            Groups = GetPersonGroups();
        }

        public async Task RefreshPersons()
        {
            Persons = await api.GetPersonList();
        }

        /// <summary>
        /// 由列表Persons更新PersonGroups，关键字为LastName
        /// </summary>
        public ObservableCollection<PersonGroup> GetPersonGroups()
        {
            var groups = new ObservableCollection<PersonGroup>();
            foreach (var person in Persons)
            {
                PersonGroup group = groups.Where(g => g.LastName == person.LastName).FirstOrDefault();
                if(group == null)
                {
                    var newGroup = new PersonGroup(person.LastName);
                    newGroup.Items.Add(person);
                    groups.Add(newGroup);
                }
                else
                {
                    group.Items.Add(person);
                }
            }
            return new ObservableCollection<PersonGroup>(groups.OrderBy(g => g.LastName));
        }
    }
}
