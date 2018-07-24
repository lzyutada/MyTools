using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;

namespace C_WMS.Data.Utility
{
    /// <summary>
    /// XML序列化操作类
    /// </summary>
    public class CWmsXmlSerializer
    {
        enum TEnumerableCatagory
        {
            EArray,
            EList,
            ENotSupported,
            EUnknown
        }
        /// <summary>
        /// 从文件中读取内容，并以string形式返回
        /// </summary>
        /// <param name="filepath">文件的完整路径和名称</param>
        /// <returns></returns>
        static public string ReadXmlFile(string filepath)
        {
            try { return File.ReadAllText(filepath, Encoding.UTF8); }
            catch (Exception ex)
            {
                MyLog.Instance.Error(ex, "从文件中读取内容异常");
                return string.Empty;
            }
        }

        #region Properties
        /// <summary>
        /// 获取flag，当前的XML反序列化是否对XML节点名称大小写敏感
        /// </summary>
        public bool IsCaseSensitive { get { return _caseSensitive; } }
        bool _caseSensitive = false;

        /// <summary>
        /// 获取被执行序列化（反序列化）操作的数据实体
        /// </summary>
        public object SerializeObj { get { return _serializeObj; } }
        object _serializeObj = null;

        /// <summary>
        /// 获取可序列化的数据类型的System.Type
        /// </summary>
        Type SerializeObjectType { get { return _type; } }
        Type _type = null;

        XmlDocument XmlDoc { get { return _xmlDoc; } }
        XmlDocument _xmlDoc = new XmlDocument();
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="caseSensitive">当前的XML反序列化是否对XML节点名称大小写敏感</param>
        public CWmsXmlSerializer(bool caseSensitive) { _caseSensitive = caseSensitive; }

        /// <summary>
        /// constructor, construct entity by specifing Type of entity for XML serializing.
        /// </summary>
        /// <param name="t">Type of entity for XML serializing.</param>
        /// <param name="caseSensitive">当前的XML反序列化是否对XML节点名称大小写敏感</param>
        public CWmsXmlSerializer(bool caseSensitive, Type t)
        {
            if (null == t)
                throw new ArgumentNullException(".cotr(Type t), invalid null for input parameter 'Type t'.");
            _caseSensitive = caseSensitive;
            _type = t;
        }

        /// <summary>
        /// constructor, construct entity by specifing Type of entity for XML serializing, using UTF-8 to encode XML content.
        /// </summary>
        /// <param name="caseSensitive">当前的XML反序列化是否对XML节点名称大小写敏感</param>
        /// <param name="t">Type of entity for XML serializing.</param>
        /// <param name="xmlDescriptor">Stream for XML content</param>
        public CWmsXmlSerializer(bool caseSensitive, Type t, string xmlDescriptor)
        {
            if (null == t)
                throw new ArgumentNullException(".cotr(Type t), invalid null for input parameter 'Type t'.");
            if (string.IsNullOrEmpty(xmlDescriptor))
                throw new ArgumentException(".cotr(string), invalid empty XML descriptor.");

            _caseSensitive = caseSensitive;
            _type = t;
            SetXmlContent(xmlDescriptor, null);
        }

        /// <summary>
        /// constructor, construct entity by specifing Type of entity for XML serializing.
        /// </summary>
        /// <param name="caseSensitive">当前的XML反序列化是否对XML节点名称大小写敏感</param>
        /// <param name="t">Type of entity for XML serializing.</param>
        /// <param name="xmlDescriptor">Stream for XML content</param>
        /// <param name="xmlEncoding">Encoding for XML content</param>
        public CWmsXmlSerializer(bool caseSensitive, Type t, string xmlDescriptor, Encoding xmlEncoding)
        {
            if (null == t)
                throw new ArgumentNullException(".cotr(Type t), invalid null for parameter 'Type t'.");
            if (null == xmlEncoding)
                throw new ArgumentNullException(".cotr(Encoding), invalid null for parameter 'Encoding xmlEncoding'.");
            if (string.IsNullOrEmpty(xmlDescriptor))
                throw new ArgumentException(".cotr(string), invalid empty XML descriptor.");

            try
            {
                _caseSensitive = caseSensitive;
                _type = t;
                SetXmlContent(xmlDescriptor, xmlEncoding);
            }
            catch (Exception ex)
            {
                MyLog.Instance.Fatal(ex, "Exception in constructing object of CWmsXmlSerializer(Type, string, Encoding)!");
            }
        }

        /// <summary>
        /// constructor, construct entity by specifing Type of entity for XML serializing.
        /// </summary>
        /// <param name="caseSensitive">当前的XML反序列化是否对XML节点名称大小写敏感</param>
        /// <param name="t">Type of entity for XML serializing.</param>
        /// <param name="xmlStream">Stream of XML content</param>
        public CWmsXmlSerializer(bool caseSensitive, Type t, Stream xmlStream)
        {
            if (null == t)
                throw new ArgumentNullException(".cotr(Type t), invalid null for input parameter 'Type t'.");
            if (null == xmlStream)
                throw new ArgumentNullException(".cotr(Stream), invalid null for input parameter 'Stream xmlStream'.");

            try
            {
                _caseSensitive = caseSensitive;
                _type = t;
                SetXmlContent(xmlStream);
            }
            catch (Exception ex)
            {
                MyLog.Instance.Fatal(ex, "Exception in constructing object of CWmsXmlSerializer(Type, Stream)!");
            }
        }

        /// <summary>
        /// 设置待反序列化的XML内容和XML内容的编码格式
        /// </summary>
        /// <param name="pXmlDes">待反序列化的XML内容</param>
        /// <param name="xmlEncoding">XML内容的编码格式</param>
        public void SetXmlContent(string pXmlDes, Encoding xmlEncoding)
        {
            try
            {
                using (var ms = new MemoryStream((null != xmlEncoding) ? xmlEncoding.GetBytes(pXmlDes) : Encoding.UTF8.GetBytes(pXmlDes)))
                {
                    ms.Position = 0;
#if MY_XML_DESERIALIZE_XMLREADER
                    _reader = XmlReader.Create(ms);
#endif
#if MY_XML_DESERIALIZE_XMLDOCUMENT
                    XmlDoc.Load(ms);
#endif
                }
            }
            catch (Exception ex)
            {
                MyLog.Instance.Fatal(ex, "Exception in setting XML content, SetXmlContent(string)!");
            }
        }

        /// <summary>
        /// 设置待反序列化的XML内容
        /// </summary>
        /// <param name="xmlStream">待反序列化的XML内容</param>
        public void SetXmlContent(Stream xmlStream)
        {
            try
            {
                xmlStream.Position = 0;
#if MY_XML_DESERIALIZE_XMLREADER
            _reader = XmlReader.Create(xmlStream);
#endif
#if MY_XML_DESERIALIZE_XMLDOCUMENT
                XmlDoc.Load(xmlStream);
#endif
            }
            catch (Exception ex)
            {
                MyLog.Instance.Fatal(ex, "Exception in setting XML content, SetXmlContent(string)!");
            }
        }

        /// <summary>
        /// 为pXmlObj执行XML序列化并返回序列化后的XML字符串内容
        /// </summary>
        /// <param name="pXmlObj">待操作的对象</param>
        /// <returns></returns>
        public string Serialize(object pXmlObj)
        {
            string xmlDes = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                //设置序序化XML格式
                XmlWriterSettings xws = new XmlWriterSettings();
                xws.Indent = true;// false;
                xws.OmitXmlDeclaration = true;
                xws.Encoding = Encoding.UTF8;

                using (XmlWriter xtw = XmlTextWriter.Create(ms, xws))
                {
                    // XML namespace setting
                    XmlSerializerNamespaces _namespaces = new XmlSerializerNamespaces(new XmlQualifiedName[] { new XmlQualifiedName(string.Empty, string.Empty) });

                    XmlSerializer xml = new XmlSerializer(pXmlObj.GetType());
                    xml.Serialize(xtw, pXmlObj, _namespaces);   // serialize obj to XML

                    // read to string.
                    ms.Seek(0, SeekOrigin.Begin);
                    XmlDoc.Load(ms);

                    XmlDeclaration xmlDecl = XmlDoc.CreateXmlDeclaration("1.0", "UTF-8", "yes");

                    xmlDes = xmlDecl.OuterXml + XmlDoc.OuterXml;
                }
            }
            return xmlDes;
        }

        /// <summary>
        /// 反序列化操作
        /// </summary>
        public object Deserialize()
        {
            if (null == XmlDoc.DocumentElement)
            {
                throw new InvalidDataException(string.Format("{0}, 没有加载XML内容", MethodBase.GetCurrentMethod().Name));
            } // 验证XML内容是否被成功加载

            //XmlRootAttribute attr = SerializeObjectType.GetCustomAttributes(true)?.First(ca => "XmlRootAttribute" == ca.GetType().Name) as XmlRootAttribute;
            var customAttrs = SerializeObjectType.GetCustomAttributes(true);
            XmlRootAttribute attr = (null != customAttrs) ? customAttrs.First(ca => "XmlRootAttribute" == ca.GetType().Name) as XmlRootAttribute : null;
            if (null == attr)
            {
                throw new XmlException(string.Format("{0}, 待反序列化的数据类型({1})不包含可序列化的XmlRoot属性", MethodBase.GetCurrentMethod().Name, SerializeObjectType));
            }

            if (NodeNameMatch(XmlDoc.DocumentElement, attr.ElementName))
            {
                try
                {
                    _serializeObj = SerializeObjectType.Assembly.CreateInstance(SerializeObjectType.FullName);

                    // 获取序列化实体的成员变量和属性
                    List<MemberInfo> miList = new List<MemberInfo>(1);
                    miList.AddRange(SerializeObj.GetType().GetFields());
                    miList.AddRange(SerializeObj.GetType().GetProperties());

                    // 解析所有子节点
                    ParseNodes(XmlDoc.DocumentElement.ChildNodes, ref _serializeObj, miList);
                }
                catch (Exception ex)
                {
                    MyLog.Instance.Error(ex, "在{0}中，反序列化{1}实体异常。", MethodBase.GetCurrentMethod().Name, SerializeObjectType);
                }
            }
            else
            {
                throw new XmlException(string.Format("{0}, 待反序列化的数据类型({1})与XML内容不匹配", MethodBase.GetCurrentMethod().Name, SerializeObjectType));
            } // if (NodeNameMatch(XmlDoc.DocumentElement, attr.ElementName)) else

            return SerializeObj;
        }

        /// <summary>
        /// 解析节点
        /// </summary>
        /// <param name="pNodeList">子节点List</param>
        /// <param name="obj">与pNodeList的ParentNode相对应的反序列化数据对象</param>
        /// <param name="pInfoList">obj的成员信息List</param>
        void ParseNodes(XmlNodeList pNodeList, ref object obj, List<MemberInfo> pInfoList)
        {
            if (null == pNodeList)
                throw new ArgumentNullException("在{0}中，非法输入参数XmlNodeList pNodeList", MethodBase.GetCurrentMethod().Name);

            foreach (XmlNode child in pNodeList)
            {
                ParseNode(child, ref obj, pInfoList);
            }
        }

        /// <summary>
        /// 解析节点
        /// </summary>
        /// <param name="pNode">当前节点元素</param>
        /// <param name="pXmlObj">与XML内容相对应的可序列化的数据类型</param>
        /// <param name="pMiList">pXmlObj的所有MemberInfo</param>
        void ParseNode(XmlNode pNode, ref object pXmlObj, List<MemberInfo> pMiList)
        {
            if (null == pNode || null == pMiList || null == pXmlObj)
                throw new ArgumentNullException("在{0}中，非法输入参数", MethodBase.GetCurrentMethod().Name);

            foreach (var mi in pMiList)
            {
                ParseMemberInfo(pNode, ref pXmlObj, mi);
            }
        }

        /// <summary>
        /// 针对当前节点和MemberInfo进行解析，并为MemberInfo所映射的属性赋值
        /// </summary>
        /// <param name="pNode">当前节点</param>
        /// <param name="pXmlObj">当前被处理的MemberInfo的DeclarationType所对应的对象</param>
        /// <param name="pInfo">当前被处理的MemberInfo</param>
        /// <exception cref="ArgumentNullException"></exception>
        void ParseMemberInfo(XmlNode pNode, ref object pXmlObj, MemberInfo pInfo)
        {
            Attribute attr = null; // pInfo的自定义属性
            // parameters validation
            if (null == pNode || null == pXmlObj || null == pInfo)
                throw new ArgumentNullException(
                    string.Format("在{0}中，非法输入参数XmlNode={1}\r\n -or- \r\nref object={2}\r\n -or- \r\nMemberInfo={3}"
                    , MethodBase.GetCurrentMethod().Name
                    , pNode, pXmlObj, pInfo)
                    );

            // 没有子节点，暂不处理
            if (null == pNode.FirstChild)
            {
                //MyLog.Instance.Info("当前节点{0}的值为空({1})", pNode.Name, pNode.OuterXml);
                return;
            } // 当前节点没有值，无需处理，即<Node/>或<Node></Node>

            // 判断是否满足反序列化赋值的条件
            bool hasXmlAttr = IsXmlAttribute(pInfo, out attr);
            bool nameMatch = NodeNameMatch(pNode, pInfo.Name);
            if (!hasXmlAttr && !nameMatch)
            {
                //MyLog.Instance.Info("{0}的成员{1}不包含XML属性或与节点({2})的名称不匹配(CaseSensitive={3}).", pXmlObj, pInfo, pNode.Name, IsCaseSensitive);
                return;
            }

            // 成员可被反序列化赋值，继续
            TEnumerableCatagory cata = GetEnumerableCatagory(pInfo);    // 判断当前成员是否为可枚举的属性，如Array或List<T>等
            if (attr is XmlElementAttribute || TEnumerableCatagory.EUnknown == cata)
            {
                ParseMemberXmlElement(pNode, ref pXmlObj, pInfo, attr as XmlElementAttribute);
            } // 当前成员变量MemberInfo被标记为[XmlElement("")]，或其Name与节点名称匹配
            else if (attr is XmlArrayAttribute || TEnumerableCatagory.EArray == cata || TEnumerableCatagory.EList == cata)
            {
                ParseMemberXmlArray(pNode, ref pXmlObj, pInfo, attr as XmlArrayAttribute);
            } // 当前成员变量MemberInfo被标记为[XmlArray("")]，或其Name与节点名称匹配
            else
            {
                MyLog.Instance.Info("{0}的成员{1}被标记[{2}]属性 -or- 未知成员属性，暂不处理.", pXmlObj, pInfo, attr);
            } // 当前成员变量MemberInfo被标记为其他属性
        }

        /// <summary>
        /// 以pNode为根节点，反序列化pXmlObj中XML属性为pAttr的成员
        /// </summary>
        /// <param name="pNode">源节点元素</param>
        /// <param name="pXmlObj">反序列化对象</param>
        /// <param name="pInfo">当前处理的pXmlObj的成员信息</param>
        /// <param name="pAttr">pInfo的XmlElement属性</param>
        void ParseMemberXmlElement(XmlNode pNode, ref object pXmlObj, MemberInfo pInfo, XmlElementAttribute pAttr)
        {
            if (null == pNode)
                throw new ArgumentNullException("在{0}中，非法输入参数XmlNode", MethodBase.GetCurrentMethod().Name);
            if (null == pXmlObj)
                throw new ArgumentNullException("在{0}中，非法输入参数ref object", MethodBase.GetCurrentMethod().Name);
            if (null == pInfo)
                throw new ArgumentNullException("在{0}中，非法输入参数MemberInfo", MethodBase.GetCurrentMethod().Name);
            //if (null == pAttr)
            //    throw new ArgumentNullException("在{0}中，非法输入参数XmlElementAttribute", MethodBase.GetCurrentMethod().Name);

            // TODO: 存在成员名称匹配且没有标记XML属性的情况，可在此做双重判断
            //       - or- 在外层为Type增加XML属性
            if (!NodeNameMatch(pNode, (null != pAttr) ? pAttr.ElementName : pInfo.Name))
            {
                //MyLog.Instance.Debug("在{0}中，成员{1}不是节点{2}期望的成员（或XML标记{3}不匹配）", MethodBase.GetCurrentMethod().Name, pInfo.Name, pNode.Name, pAttr);
                return;
            } // 当前MemberInfo不是pNode期望的成员

            if (pNode.FirstChild is XmlText)
            {
                SetTextValue(ref pXmlObj, pInfo, pNode.InnerText);
            } // 当前XML节点为叶子节点，即<Node>value</Node>
            else if (pNode.FirstChild is XmlElement)
            {
                SetElementValue(pNode, ref pXmlObj, pInfo);
            } // 当前XML节点为中间节点，即<Node><Child></Child><Child/><Child>Value</Child></Node>
        }

        /// <summary>
        /// 以pNode为根节点，反序列化pXmlObj中XML属性为pAttr的成员
        /// </summary>
        /// <param name="pNode">源节点元素</param>
        /// <param name="pXmlObj">反序列化对象</param>
        /// <param name="pInfo">当前处理的pXmlObj的成员信息</param>
        /// <param name="pAttr">XmlArray属性</param>
        void ParseMemberXmlArray(XmlNode pNode, ref object pXmlObj, MemberInfo pInfo, XmlArrayAttribute pAttr)
        {
            if (null == pNode)
                throw new ArgumentNullException("在{0}中，非法输入参数XmlNode", MethodBase.GetCurrentMethod().Name);
            if (null == pXmlObj)
                throw new ArgumentNullException("在{0}中，非法输入参数ref object", MethodBase.GetCurrentMethod().Name);
            if (null == pInfo)
                throw new ArgumentNullException("在{0}中，非法输入参数MemberInfo", MethodBase.GetCurrentMethod().Name);
            if (null == pAttr)
                throw new ArgumentNullException("在{0}中，非法输入参数XmlElementAttribute", MethodBase.GetCurrentMethod().Name);

            if (!NodeNameMatch(pNode, pAttr.ElementName))
            {
                return;
            } // 当前MemberInfo不是pNode期望的成员

            switch (GetEnumerableCatagory(pInfo))
            {
                case TEnumerableCatagory.EArray:
                    {
                        ParseArrayMemberXmlArray(pNode, ref pXmlObj, pInfo, pAttr);
                        break;
                    }
                case TEnumerableCatagory.EList:
                    {
                        ParseListMemberXmlArray(pNode, ref pXmlObj, pInfo);
                        break;
                    }
                case TEnumerableCatagory.EUnknown:
                    {
                        MyLog.Instance.Warning("在{0}中，{1}可能是未知的可Enumerable反序列化的成员", MethodBase.GetCurrentMethod().Name, pInfo);
                        break;
                    }
                default:
                    throw new NotSupportedException(string.Format("在{0}中，{1}不可被作为可Enumerable反序列化的成员", MethodBase.GetCurrentMethod().Name, pInfo));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pNode"></param>
        /// <param name="pXmlObj"></param>
        /// <param name="pInfo"></param>
        void ParseListMemberXmlArray(XmlNode pNode, ref object pXmlObj, MemberInfo pInfo)
        {
            if (null == pNode)
                throw new ArgumentNullException("在{0}中，非法输入参数XmlNode", MethodBase.GetCurrentMethod().Name);
            if (null == pXmlObj)
                throw new ArgumentNullException("在{0}中，非法输入参数ref object", MethodBase.GetCurrentMethod().Name);
            if (null == pInfo)
                throw new ArgumentNullException("在{0}中，非法输入参数MemberInfo", MethodBase.GetCurrentMethod().Name);

            // get generic arguments of pInfo
            Type[] genericTypes = null;
            switch (pInfo.MemberType)
            {
                case MemberTypes.Field: { genericTypes = (pInfo as FieldInfo).FieldType.GetGenericArguments(); break; }
                case MemberTypes.Property: { genericTypes = (pInfo as PropertyInfo).PropertyType.GetGenericArguments(); break; }
                default: throw new NotSupportedException(string.Format("在{0}中，{1}不可被作为反序列化的成员（MemberType={2}）", MethodBase.GetCurrentMethod().Name, pInfo, pInfo.MemberType));
            }

            // get the first generic type of pInfo
            var genericType = genericTypes?[0]; // (null != genericTypes && 0 < genericTypes.Length) ? genericTypes[0] : null;
            if (null == genericType)
                throw new NotSupportedException(string.Format("在{0}中，({1})获取List<T>的泛型类型失败, GenericArguments={2}", MethodBase.GetCurrentMethod().Name, pInfo, genericTypes));

            // get fields and properties of the generic argument
            List<MemberInfo> miList = new List<MemberInfo>(1);
            miList.AddRange(genericType.GetFields());
            miList.AddRange(genericType.GetProperties());

            // create instance of List<T>
            Type memberInfoType = CWmsReflectionHelper.GetMemberInfoType(pInfo);
            var arrayMemberObj = Activator.CreateInstance(memberInfoType);

            // envaluated for the created instance from current XmlNode.
            foreach (XmlNode child in pNode.ChildNodes)
            {
                var memberObj = genericType.Assembly.CreateInstance(genericType.FullName);
                ParseNodes(child.ChildNodes, ref memberObj, miList);

                MethodInfo methodInfo = arrayMemberObj.GetType().GetMethod("Add", BindingFlags.Instance | BindingFlags.Public);
                methodInfo.Invoke(arrayMemberObj, new object[] { memberObj });
            }

            // envaluated for pXmlObj
            //(pInfo as FieldInfo).SetValue(pXmlObj, arrayMemberObj);

            if (pInfo is FieldInfo)
            {
                (pInfo as FieldInfo).SetValue(pXmlObj, Convert.ChangeType(arrayMemberObj, (pInfo as FieldInfo).FieldType));
            } // 当前MemberInfo是成员变量
            else if (pInfo is PropertyInfo)
            {
                (pInfo as PropertyInfo).SetValue(pXmlObj, Convert.ChangeType(arrayMemberObj, (pInfo as PropertyInfo).PropertyType), null);
            } // 当前MemberInfo是属性
            else
            {
                throw new InvalidCastException(string.Format("{0}不是期望的MemberInfo, 不应被指定XmlElement属性", pInfo));
            } // 当前MemberInfo是不支持的
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pNode"></param>
        /// <param name="pXmlObj"></param>
        /// <param name="pInfo"></param>
        /// <param name="pAttr"></param>
        void ParseArrayMemberXmlArray(XmlNode pNode, ref object pXmlObj, MemberInfo pInfo, XmlArrayAttribute pAttr)
        {
            if (null == pNode)
                throw new ArgumentNullException("在{0}中，非法输入参数XmlNode", MethodBase.GetCurrentMethod().Name);
            if (null == pXmlObj)
                throw new ArgumentNullException("在{0}中，非法输入参数ref object", MethodBase.GetCurrentMethod().Name);
            if (null == pInfo)
                throw new ArgumentNullException("在{0}中，非法输入参数MemberInfo", MethodBase.GetCurrentMethod().Name);

            // get generic arguments of pInfo
            Type memberType = null;
            switch (pInfo.MemberType)
            {
                case MemberTypes.Field: { memberType = (pInfo as FieldInfo).FieldType; break; }
                case MemberTypes.Property: { memberType = (pInfo as PropertyInfo).PropertyType; break; }
                default: throw new NotSupportedException(string.Format("在{0}中，{1}不可被作为反序列化的成员（MemberType={2}）", MethodBase.GetCurrentMethod().Name, pInfo, pInfo.MemberType));
            }

            // create instance of List<T> by T[]
            var typeFullName = memberType.FullName.Remove(memberType.FullName.IndexOf("[]"), 2);
            var genericType = Assembly.GetAssembly(memberType)?.GetType(typeFullName, true);
            if (null == genericType)
                throw new NullReferenceException(string.Format("在{0}中，无法根据{1}获取Type", MethodBase.GetCurrentMethod().Name, typeFullName));
            Type[] typeArgs = { genericType };
            var makeme = typeof(List<>).MakeGenericType(typeArgs);
            var arrayMemberObj = Activator.CreateInstance(makeme);  // the new created instance

            // get members of T
            List<MemberInfo> miList = new List<MemberInfo>(1);
            miList.AddRange(genericType.GetFields());
            miList.AddRange(genericType.GetProperties());

            // envaluated members of arrayMemberObj from pNode and its children
            foreach (XmlNode child in pNode.ChildNodes)
            {
                var memberObj = genericType.Assembly.CreateInstance(genericType.FullName);
                ParseNodes(child.ChildNodes, ref memberObj, miList);
                MethodInfo methodInfo = arrayMemberObj.GetType().GetMethod("Add", BindingFlags.Instance | BindingFlags.Public);
                methodInfo.Invoke(arrayMemberObj, new object[] { memberObj });
            }

            // convert List<T> to T[]
            MethodInfo methodInfo_ToList = arrayMemberObj.GetType().GetMethod("ToArray", BindingFlags.Instance | BindingFlags.Public);
            var array = methodInfo_ToList.Invoke(arrayMemberObj, new object[] { });
            //(pInfo as PropertyInfo).SetValue(pXmlObj, array, null);

            if (pInfo is FieldInfo)
            {
                (pInfo as FieldInfo).SetValue(pXmlObj, Convert.ChangeType(array, (pInfo as FieldInfo).FieldType));
            } // 当前MemberInfo是成员变量
            else if (pInfo is PropertyInfo)
            {
                (pInfo as PropertyInfo).SetValue(pXmlObj, Convert.ChangeType(array, (pInfo as PropertyInfo).PropertyType), null);
            } // 当前MemberInfo是属性
            else
            {
                throw new InvalidCastException(string.Format("{0}不是期望的MemberInfo, 不应被指定XmlElement属性", pInfo));
            } // 当前MemberInfo是不支持的
        }

        /// <summary>
        /// 根据源节点pNode为目标对象pXmlObj的成员（pInfo）赋值
        /// </summary>
        /// <param name="pValue">赋值源</param>
        /// <param name="pXmlObj">目标对象</param>
        /// <param name="pInfo">目标对象pXmlObj的成员</param>
        void SetTextValue(ref object pXmlObj, MemberInfo pInfo, object pValue)
        {
            if (null == pXmlObj)
                throw new ArgumentNullException("在{0}中，非法输入参数ref object", MethodBase.GetCurrentMethod().Name);
            if (null == pInfo)
                throw new ArgumentNullException("在{0}中，非法输入参数MemberInfo", MethodBase.GetCurrentMethod().Name);

            if (pInfo is FieldInfo)
            {
                (pInfo as FieldInfo).SetValue(pXmlObj, Convert.ChangeType(pValue, (pInfo as FieldInfo).FieldType));
            } // 当前MemberInfo是成员变量
            else if (pInfo is PropertyInfo)
            {
                (pInfo as PropertyInfo).SetValue(pXmlObj, Convert.ChangeType(pValue, (pInfo as PropertyInfo).PropertyType), null);
            } // 当前MemberInfo是属性
            else
            {
                throw new InvalidCastException(string.Format("{0}不是期望的MemberInfo, 不应被指定XmlElement属性", pInfo));
            } // 当前MemberInfo是不支持的
        }

        /// <summary>
        /// 根据源节点pNode为目标对象pXmlObj的成员（pInfo）赋值
        /// </summary>
        /// <param name="pNode">源节点元素</param>
        /// <param name="pXmlObj">目标对象</param>
        /// <param name="pInfo">目标对象pXmlObj的成员</param>
        void SetElementValue(XmlNode pNode, ref object pXmlObj, MemberInfo pInfo)
        {
            if (null == pNode)
                throw new ArgumentNullException("在{0}中，非法输入参数XmlNode", MethodBase.GetCurrentMethod().Name);
            if (null == pXmlObj)
                throw new ArgumentNullException("在{0}中，非法输入参数ref object", MethodBase.GetCurrentMethod().Name);
            if (null == pInfo)
                throw new ArgumentNullException("在{0}中，非法输入参数MemberInfo", MethodBase.GetCurrentMethod().Name);

            // create member object instance
            object memberObj = null;
            if (pInfo is FieldInfo)
            {
                memberObj = (pInfo as FieldInfo).FieldType.Assembly.CreateInstance((pInfo as FieldInfo).FieldType.FullName);
            }
            else if (pInfo is PropertyInfo)
            {
                memberObj = (pInfo as PropertyInfo).PropertyType.Assembly.CreateInstance((pInfo as FieldInfo).FieldType.FullName);
            }
            else
            {
                throw new InvalidCastException(string.Format("{0}不是期望的MemberInfo, 不应被指定XmlElement属性", pInfo));
            } // 当前MemberInfo是不支持的

            // get MemberInfo list of memberObj
            List<MemberInfo> miList = new List<MemberInfo>(1);
            miList.AddRange(memberObj.GetType().GetFields());
            miList.AddRange(memberObj.GetType().GetProperties());

            // parse nodes for memberObj
            ParseNodes(pNode.ChildNodes, ref memberObj, miList);

            // set value of member of pXmlObj with memberObj
            SetTextValue(ref pXmlObj, pInfo, memberObj);
        }

        /// <summary>
        /// 获取支持Enumerable反序列化操作的PInfo的Enumerable类别
        /// </summary>
        /// <param name="pInfo">成员信息</param>
        /// <returns></returns>
        TEnumerableCatagory GetEnumerableCatagory(MemberInfo pInfo)
        {
            switch (pInfo.MemberType)
            {
                case MemberTypes.Field:
                    {
                        var fieldType = (pInfo as FieldInfo).FieldType;
                        return (fieldType.IsArray) ? TEnumerableCatagory.EArray : ((fieldType.IsGenericType && fieldType.Name.Equals("List`1")) ? TEnumerableCatagory.EList : TEnumerableCatagory.EUnknown);
                    }
                case MemberTypes.Property:
                    {
                        var propertyType = (pInfo as PropertyInfo).PropertyType;
                        return (propertyType.IsArray) ? TEnumerableCatagory.EArray : ((propertyType.IsGenericType && propertyType.Name.Equals("List`1")) ? TEnumerableCatagory.EList : TEnumerableCatagory.EUnknown);
                    }
                default:
                    return TEnumerableCatagory.ENotSupported;
            }
        }

        /// <summary>
        /// 比较pAttrElementName和XML节点pNode的节点名称是否一致。
        /// 字符串比较是否大小写敏感根据IsCaseSensitive而定。
        /// </summary>
        /// <param name="pNode">XMLNode节点</param>
        /// <param name="pAttrElementName">待比较的字符串内容</param>
        /// <returns></returns>
        bool NodeNameMatch(XmlNode pNode, string pAttrElementName)
        {
            return (null == pNode) ? false : ((IsCaseSensitive) ? pNode.Name.Equals(pAttrElementName) : pNode.Name.Equals(pAttrElementName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 判断成员信息pInfo是否可被标记为XmlArray属性
        /// </summary>
        /// <param name="pInfo">成员信息</param>
        /// <returns></returns>
        bool IsXmlArray(MemberInfo pInfo)
        {
            if (null == pInfo)
                throw new ArgumentNullException(""); // TODO: exception message

            var fieldType = (pInfo as FieldInfo).FieldType;
            return (
                pInfo.GetType().IsArray
                || (fieldType.IsGenericType && fieldType.Name.Equals("List`1"))
                );
        }

        /// <summary>
        /// 判断成员信息pInfo是否被标记为XML相关属性
        /// </summary>
        /// <param name="pInfo">成员信息</param>
        /// <param name="attr">返回XML相关属性</param>
        /// <returns></returns>
        bool IsXmlAttribute(MemberInfo pInfo, out Attribute attr)
        {
            bool found = false;
            attr = null;

            var caList = pInfo.GetCustomAttributes(true);
            foreach (var ca in caList)
            {
                if (ca is XmlElementAttribute)
                {
                    attr = new XmlElementAttribute((ca as XmlElementAttribute).ElementName);
                    found = true;
                    break;
                }
                else if (ca is XmlArrayAttribute)
                {
                    attr = new XmlArrayAttribute((ca as XmlArrayAttribute).ElementName);
                    found = true;
                    break;
                }
                else
                {
                    // TODO: other type
                }
            }
            return found;
        }
    }
}
