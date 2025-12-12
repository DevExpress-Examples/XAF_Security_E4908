using DevExpress.Persistent.Base;
using System;
using System.Text;

namespace BusinessObjectsLibrary.BusinessObjects {
    public class PersonImpl {
		//private const string defaultFullNameFormat = "{FirstName} {MiddleName} {LastName}";
		//private const string defaultFullNameServerModeExpression = "FirstName + MiddleName + LastName";
		// It is necessary to initialize these fields by not null values,
		// else the [PersistentAlias("FirstName + MiddleName + LastName")] does not work correctly in the Server mode.
		private string firstName = "";
		private string lastName = "";
		private string middleName = "";
		private DateTime birthday;
		private string email = "";
		private static string fullNameFormat;// = defaultFullNameFormat;
//		private static string fullNameServerModeExpression = defaultFullNameFormat;

		//private bool IsOldFormat(string formatStr) {
		//    if((formatStr.IndexOf("LastName", StringComparison.InvariantCultureIgnoreCase) >=0)
		//        && (formatStr.IndexOf("{LastName", StringComparison.InvariantCultureIgnoreCase) < 0)
		//        ||
		//        (formatStr.IndexOf("FirstName", StringComparison.InvariantCultureIgnoreCase) >= 0)
		//        && (formatStr.IndexOf("{FirstName", StringComparison.InvariantCultureIgnoreCase) < 0)) {
		//        return true;
		//    }
		//    return false;
		//}
		private static string ReplaceIgnoreCase(string str, string oldString, string newString) {
			string result = "";
			int lastNameEntryStartIndex = str.IndexOf(oldString, StringComparison.InvariantCultureIgnoreCase);
			if((lastNameEntryStartIndex >= 0)
				&& (str.IndexOf(newString, StringComparison.InvariantCulture) < 0)) {
				if(lastNameEntryStartIndex > 0) {
					result = str.Substring(0, lastNameEntryStartIndex);
				}
				result += newString;
				int oldStringLength = oldString.Length;
				if((lastNameEntryStartIndex + oldStringLength) < str.Length) {
					result += str.Substring(lastNameEntryStartIndex + oldStringLength, str.Length - lastNameEntryStartIndex - oldStringLength);
				}
			}
			else {
				result = str;
			}
			return result;
		}
		private static string ConvertIfItIsInOldFormat(string formatStr) {
			string result = ReplaceIgnoreCase(formatStr, "FirstName", "{FirstName}");
			result = ReplaceIgnoreCase(result, "LastName", "{LastName}");
			return ReplaceIgnoreCase(result, "MiddleName", "{MiddleName}");
		}
		//public static void SetFullNameFormat(string format, string serverModeExpression) {
		//	FullNameFormat = format;
		//	fullNameServerModeExpression = serverModeExpression;
		//}
		//public static string FullNamePersistentAlias {
		//	get { return fullNameServerModeExpression; }
		//}
		public static string FullNameFormat {
			get { return fullNameFormat; }
			set {
				fullNameFormat = value;
				//if(string.IsNullOrEmpty(fullNameFormat)) {
				//	fullNameFormat = defaultFullNameFormat;
				//}
				//else {
				fullNameFormat = ConvertIfItIsInOldFormat(fullNameFormat);
				//}
			}
		}

        public void SetFullName(string fullName) {
			FirstName = MiddleName = LastName = "";
			int index = fullName.IndexOf(',');
			if(index > 0) { // LastName, FirstName MiddleName -> FirstName MiddleName LastName
#if NET
                fullName = string.Concat(fullName.Remove(0, index + 1).Trim(), " ", fullName.AsSpan(0, index));
#else
                fullName = fullName.Remove(0, index + 1).Trim() + " " + fullName.Substring(0, index);
#endif
            }
			string[] names = fullName.Split(' ');
			FirstName = names[0];
			if(names.Length == 2) {
				LastName = names[1];
			}
			else
				if(names.Length == 3) {
					MiddleName = names[1];
					LastName = names[2];
				}
				else {
					for(int i = 2; i < names.Length; i++) {
						LastName += " " + names[i];
					}
				}
		}
		public string FirstName {
			get { return firstName; }
			set { firstName = value; }
		}
		public string LastName {
			get { return lastName; }
			set { lastName = value; }
		}
		public string MiddleName {
			get { return middleName; }
			set { middleName = value; }
		}
        public DateTime Birthday {
            get { return birthday; }
            set { birthday = value; }
        }
		public string FullName {
			get { return ObjectFormatter.Format(fullNameFormat, this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty); }
		}
		public string Email {
			get { return email; }
			set { email = value; }
		}
    }
}
