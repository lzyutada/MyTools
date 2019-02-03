using MangoPublishPackage.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MangoPublishPackage
{
    public class Utility
    {
        static public int FindFile(string pRoot, string pPattern, out List<FileInfo> pRetList)
        {
            if (!Directory.Exists(pRoot))
            {
                pRetList = null;
                //BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateToolstripStatusLabel), null, string.Format("![ERROR, 查找文件: {0}], 路径({1})不存在", pPattern, pRoot));
                return -2;
            }
            else
            {
                //BeginInvoke(new _dlgtFunc_UpdateFormControl(DlgtFunc_UpdateToolstripStatusLabel), null, string.Format("正在查找路径: {0}", pRoot));
            }

            pRetList = new List<FileInfo>();
            DirectoryInfo diRoot = new DirectoryInfo(pRoot);
            DirectoryInfo[] subDirs = diRoot.GetDirectories();
            foreach (DirectoryInfo subdir in subDirs)
            {
                List<FileInfo> tmpList = null;
                if (0 < FindFile(subdir.FullName, pPattern, out tmpList) && tmpList.EnumerableAny())
                {
                    pRetList.AddRange(tmpList);
                }
            }

            FileInfo[] files = diRoot.GetFiles(pPattern);
            if (files.EnumerableAny())
                pRetList.AddRange(files);
            return (pRetList?.Count).Int();
        }

        static public int FindFile(string pRoot, string pPattern, IEnumerable<string> pIgnorFolders, IEnumerable<string> pIgnorFiles, out List<FileInfo> pRetList)
        {
            if (!Directory.Exists(pRoot))
            {
                pRetList = null;
                return -2;
            }

            pRetList = new List<FileInfo>();
            DirectoryInfo diRoot = new DirectoryInfo(pRoot);
            DirectoryInfo[] subDirs = diRoot.GetDirectories();
            foreach (DirectoryInfo subdir in subDirs)
            {
                if (pIgnorFolders.EnumerableContains(subdir.Name))
                    continue;
                List<FileInfo> tmpList = null;
                if (0 < FindFile(subdir.FullName, pPattern, pIgnorFolders, pIgnorFiles, out tmpList) && tmpList.EnumerableAny())
                {
                    pRetList.AddRange(tmpList);
                }
            }

            FileInfo[] files = diRoot.GetFiles(pPattern);
            foreach (FileInfo file in files)
            {
                if (pIgnorFiles.EnumerableContains(file.Name))
                    continue;
                pRetList.Add(file);
            }
            //if (files.EnumerableAny())
            //    pRetList.AddRange(files);
            return (pRetList?.Count).Int();
        }

        static public IEnumerable<FileInfo> FindFile(string pRoot, string pPattern)
        {
            if (!Directory.Exists(pRoot))
            {
                return Enumerable.Empty<FileInfo>();
            }

            List<FileInfo> retList = new List<FileInfo>(3);
            DirectoryInfo diRoot = new DirectoryInfo(pRoot);
            DirectoryInfo[] subDirs = diRoot.GetDirectories();
            foreach (DirectoryInfo subdir in subDirs)
            {
                IEnumerable<FileInfo> tmpFiles = FindFile(subdir.FullName, pPattern);
                if (tmpFiles.EnumerableAny())
                {
                    retList.AddRange(tmpFiles);
                }
            }

            FileInfo[] files = diRoot.GetFiles(pPattern);
            if (files.EnumerableAny())
                retList.AddRange(files);
            return retList;
        }

        static public IEnumerable<string> StrMatchWithRegExp(string pattern, string source)
        {
            Regex reg = new Regex(pattern, RegexOptions.IgnoreCase); // 搜索匹配的字符串
            MatchCollection matchList = reg.Matches(source);
            return matchList.OfType<string>();
        }

        static public long ReadFile(string filename, out byte[] buffer)
        {
            try
            {
                long readCount = 0;
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        string fileContent = sr.ReadToEnd();
                        buffer = Encoding.UTF8.GetBytes(fileContent);
                        readCount = (buffer?.Length).Int();
                    }
                }
                return readCount;
            }
            catch (Exception)
            {
                buffer = null;
                return 0;
            }
        }

        /// <summary>
        /// 返回值: 0 - 相等; 负数 - left小于right; 证书 - left大于right
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        static public long CompareBytes(byte[] left, byte[] right)
        {
            if (null == left && null == right) return 0;
            else if (null == left) return -1;
            else if (null == right) return 1;
            else
            {
                for (long i = 0; i < left.LongLength; i++)
                {
                    if (i > right.LongLength || left[i] > right[i]) return i;
                    else if (left[i] < right[i]) return i * -1;
                }
                return 0;
            }
        }
    }

    static public class MyExtensionHelp
    { 
        static public bool EnumerableAny<T>(this IEnumerable<T> group)
        {
            return null != group && Enumerable.Any(group);
        }

        static public bool EnumerableContains<T>(this IEnumerable<T> group, T Value)
        {
            return (null != group && group.Contains(Value));
        }

        static public int Int(this object Value)
        {
            try { return (null == Value) ? 0 : int.Parse(Value.ToString()); }
            catch (Exception) { return 0; }
        }
    }
}
