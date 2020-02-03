using System;
using System.Collections.Generic;
using System.Linq;

namespace ASPNETCoreODataService {
	public static class TypeSystem {
		readonly static Dictionary<KeyValuePair<Type, Type>, bool> convertableType = new Dictionary<KeyValuePair<Type, Type>, bool>();
		internal static Type GetElementType(Type seqType) {
			Type ienum = FindIEnumerable(seqType);
			if(ienum == null) return seqType;
			return ienum.GetGenericArguments()[0];
		}
		private static Type FindIEnumerable(Type seqType) {
			if(seqType == null || seqType == typeof(string))
				return null;
			if(seqType.IsArray)
				return typeof(IEnumerable<>).MakeGenericType(seqType.GetElementType());
			if(seqType.IsGenericType) {
				foreach(Type arg in seqType.GetGenericArguments()) {
					Type ienum = typeof(IEnumerable<>).MakeGenericType(arg);
					if(ienum.IsAssignableFrom(seqType)) {
						return ienum;
					}
				}
			}
			Type[] ifaces = seqType.GetInterfaces();
			if(ifaces != null && ifaces.Length > 0) {
				foreach(Type iface in ifaces) {
					Type ienum = FindIEnumerable(iface);
					if(ienum != null) return ienum;
				}
			}
			if(seqType.BaseType != null && seqType.BaseType != typeof(object)) {
				return FindIEnumerable(seqType.BaseType);
			}
			return null;
		}

		public static string ProcessTypeName(string namespaceName, Type type) {
			return ProcessTypeName(namespaceName, type.FullName);
		}
		public static string ProcessTypeName(string namespaceName, string typeName) {
			string name;
			if(typeName.Length > namespaceName.Length && typeName.Substring(0, namespaceName.Length) == namespaceName)
				name = typeName.Substring(namespaceName.Length + 1).Replace(".", "_").Replace("+", "_");
			else
				name = typeName.Replace(".", "_").Replace("+", "_");
			return name;
		}
		public static bool AreConvertable(Type type1, Type type2) {
			if(type1 == null || type2 == null) return false;
			bool isExists;
			lock(convertableType) {
				if(convertableType.TryGetValue(new KeyValuePair<Type, Type>(type1, type2), out isExists)) return isExists;
				isExists = CheckConvertable(type1, type2);
				convertableType.Add(new KeyValuePair<Type, Type>(type1, type2), isExists);
			}
			return isExists;
		}

		static bool CheckConvertable(Type type1, Type type2) {
			if(type1 == type2) return true;
			Type tempType = type2;
			do {
				if(tempType.IsAssignableFrom(type1))
					return true;
				tempType = tempType.BaseType;
			} while(tempType != null && tempType != typeof(object));

			tempType = type1;
			do {
				if(tempType.IsAssignableFrom(type2))
					return true;
				tempType = tempType.BaseType;
			} while(tempType != null && tempType != typeof(object));

			return false;

		}

		internal static bool IsQueryableType(Type collectionType) {
			if(!collectionType.IsGenericTypeDefinition) {
				if(!collectionType.IsGenericType) return false;
				collectionType = collectionType.GetGenericTypeDefinition();
			}
			return typeof(IQueryable<>).IsAssignableFrom(collectionType) || collectionType.GetInterfaces().Any(inf => inf.IsGenericType && typeof(IQueryable<>) == inf.GetGenericTypeDefinition());
		}

		internal static bool IsEnumerableType(Type collectionType) {
			if(!collectionType.IsGenericTypeDefinition) {
				if(!collectionType.IsGenericType) return false;
				collectionType = collectionType.GetGenericTypeDefinition();
			}
			return typeof(IEnumerable<>).IsAssignableFrom(collectionType) || collectionType.GetInterfaces().Any(inf => inf.IsGenericType && typeof(IEnumerable<>) == inf.GetGenericTypeDefinition());
		}
	}
}