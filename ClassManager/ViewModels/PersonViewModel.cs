using ClassManager.Models;
using ClassManager.Networks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Threading;

namespace ClassManager.ViewModels
{
    public class PersonViewModel: BaseViewModel
    {
        APIService api = new APIService();

        
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
        public Person PersonOnDisplay {
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

        /// <summary>
        /// 删除正在展示的学生信息，并同步列表
        /// </summary>
        /// <returns>删除是否成功</returns>
        public async Task<bool> DeletePersonOnDisplay()
        {
            bool result = await api.DeletePerson(PersonOnDisplay);
            if (result)
            {
                await this.Init();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 提交对正在展示的学生信息的修改
        /// </summary>
        /// <param name="target_StudentNubmer">目标原学号</param>
        /// <returns></returns>
        public async Task<bool> UpdatePersonOnDisplay(string target_StudentNubmer)
        {
            return await api.UpdatePerson(target_StudentNubmer, PersonOnDisplay);
        }

        /// <summary>
        /// 提交新增学生信息
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AddPersonOnDisplay()
        {
            return await api.AddPerson(PersonOnDisplay);
        }

        /// <summary>
        /// 根据<paramref name="searchText"/>过滤<see cref="Groups"/>
        /// </summary>
        /// <param name="searchText">查询关键字</param>
        public void GroupsFilter(string searchText)
        {
            PersonOnDisplay = Persons.FirstOrDefault();
            Groups.Clear();
            foreach(var person in Persons)
            {
                if (person.Name.Contains(searchText) ||
                    person.StudentNumber.Contains(searchText) ||
                    person.Dormitory.Contains(searchText) || 
                    person.NativeProvince.Equals(searchText) )
                {
                    PersonGroup group = Groups.Where(g => g.LastName == person.LastName).FirstOrDefault();
                    if (group == null)
                    {
                        var newGroup = new PersonGroup(person.LastName);
                        newGroup.Items.Add(person);
                        Groups.Add(newGroup);
                    }
                    else
                    {
                        group.Items.Add(person);
                    }
                }
            }
        }
    }
}
