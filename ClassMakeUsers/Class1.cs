using System;
using System.DirectoryServices;
using System.Collections.Generic;

namespace ClassCreateUsers
{
    public class CreateUsers
    {

        static public void MakeUser(string Name, string Pass)
        {
            DirectoryEntry AD = new DirectoryEntry("WinNT://" + Environment.MachineName + ",computer");
            DirectoryEntry NewUser = AD.Children.Add(Name, "user");
            NewUser.Invoke("SetPassword", new object[] { Pass });
            NewUser.Invoke("Put", new object[] { "Description", "Test User from .NET" });
            NewUser.CommitChanges();
            DirectoryEntry grp;

            grp = AD.Children.Find("Гости", "group");
            if (grp != null)
            {
                grp.Invoke("Add", new object[] { NewUser.Path.ToString() });
            }
        }


        static public void RemoveUser(string Name)
        {
            DirectoryEntry localDirectory = new DirectoryEntry("WinNT://" + Environment.MachineName.ToString());
            DirectoryEntries users = localDirectory.Children;
            DirectoryEntry user = users.Find($"{Name}");
            users.Remove(user);
        }


        static public List<string> ListUsers()
        {
            List<string> users = new List<string>();
            DirectoryEntry computerEntry = new DirectoryEntry("WinNT://" + Environment.MachineName.ToString());

            foreach (DirectoryEntry childEntry in computerEntry.Children)
                if (childEntry.SchemaClassName == "User")
                {
                    users.Add(childEntry.Name);
                }
            return users;
        }
    }
}
