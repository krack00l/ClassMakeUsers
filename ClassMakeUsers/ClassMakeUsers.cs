using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;

namespace ClassCreateUsers
{
    public class CreateUsers
    {

        private const string USER = "User";
        private const string PATH = "WinNT://";
        private const string DESCRIPTION = "Description";
        private const string PASSWORD = "SetPassword";
        private const string GROUP = "group";
        private const string PUT = "Put";
        private const string ADD = "Add";


        public static void MakeUser(string Name, string Pass, string Group, string description = "")
        {
            DirectoryEntry AD = new DirectoryEntry(PATH + Environment.MachineName);
            DirectoryEntry NewUser = AD.Children.Add(Name, USER);
            NewUser.Invoke(PASSWORD, new object[] { Pass });
            NewUser.Invoke(PUT, new object[] { DESCRIPTION, description });
            NewUser.CommitChanges();

            if (Group != "")
            {
                IncludeInGroup(AD, NewUser, Group);
            }
        }


        private static void IncludeInGroup(DirectoryEntry AD, DirectoryEntry NewUser,string Group)
        {
            DirectoryEntry grp;
            grp = AD.Children.Find(Group, GROUP);
            if (grp != null)
            {
                grp.Invoke(ADD, new object[] { NewUser.Path.ToString() });
            }
        }


        public static void RemoveUser(string Name)
        {
            DirectoryEntry localDirectory = new DirectoryEntry(PATH + Environment.MachineName.ToString());
            DirectoryEntries users = localDirectory.Children;
            DirectoryEntry user = users.Find(Name);
            users.Remove(user);
        }


        private static void Change(string Name, string Parameter, string Mode)
        {
            DirectoryEntry localDirectory = new DirectoryEntry(PATH + Environment.MachineName.ToString());
            DirectoryEntries users = localDirectory.Children;
            DirectoryEntry user = users.Find(Name);
            if (Mode == DESCRIPTION)
            {
                user.Invoke(PUT, new object[] { DESCRIPTION, Parameter });
            }
            if (Mode == PASSWORD)
            {
                user.Invoke(PASSWORD, new object[] { Parameter });
            }
            user.CommitChanges();
        }



        public static void ChangeDescription(string Name, string description)
        {
            Change(Name, description, DESCRIPTION);
        }

        public static void ChangePassword(string Name, string Pass)
        {
            Change(Name, Pass, PASSWORD);
        }




        private static List<string> ListView(string Mode)
        {
            List<string> users = new List<string>();
            DirectoryEntry computerEntry = new DirectoryEntry(PATH + Environment.MachineName.ToString());
            foreach (DirectoryEntry childEntry in computerEntry.Children)
                if (childEntry.SchemaClassName == Mode)
                {
                    users.Add(childEntry.Name);
                }
            return users;
        }

        public static List<string> ListGroup()
        {
            return ListView(GROUP);
        }

        public static List<string> ListUsers()
        {
            return ListView(USER);
        }
    }
}
