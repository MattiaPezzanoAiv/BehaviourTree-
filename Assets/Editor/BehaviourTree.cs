using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;
using System;
using System.Xml;
using System.Linq;
using System.Xml.Serialization;
using System.Text;


namespace Mattia
{

    public class BehaviourTree : EditorWindow
    {

        private UnityEngine.Object btFile;
        private Vector2 defaultSize = new Vector2(110, 20);
        private Vector2 lastPos;
        private float scrollHeight;
        private float scrollWidth;

        private int currentTabCount;
        private string filepath;
        private Vector2 scrollPosition;
        private static BehaviourTree window;
        private string newFileCreatedName = "";
        private string defaultPathForSavingXmlFile = "Assets/BehaviourTreeXml/";
        private string defaultXmlFirstLine = "<?xml version=\"1.0\" encoding=\"Windows-1252\"?>";

        private Node DeserializedNode { get; set; }

        private bool ModifiedTreeLastFrame { get; set; }

        [MenuItem("BehaviourTree/Editor")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            window = (BehaviourTree)EditorWindow.GetWindow(typeof(BehaviourTree));
            window.Show();
            window.maximized = true;
        }

        void Update()
        {
            Repaint();
        }
        void OnGUI()
        {
            //reset gui variables
            scrollHeight = 0;
            scrollWidth = 0;
            lastPos = new Vector2(5, 5);
            //start code

            if (DeserializedNode == null)
            {

                if (btFile == null)
                {
                    newFileCreatedName = GUI.TextField(new Rect(lastPos, defaultSize + new Vector2(30, 0)), newFileCreatedName);
                    Tab(200);
                    if (GUI.Button(new Rect(lastPos, defaultSize + new Vector2(30, 10)), new GUIContent("New Tree")))
                    {
                        CreateNewXmlFile();
                    }

                    AddLine(80);
                    Rect pos = new Rect(new Vector2((position.width / 2) - (position.width / 2) / 2, lastPos.y), new Vector2(position.width / 2, position.height * 0.05f));
                    btFile = EditorGUI.ObjectField(pos, btFile, typeof(UnityEngine.Object), false);
                }

                if (btFile == null) return;

                filepath = AssetDatabase.GetAssetPath(btFile);
                DeserializedNode = BehaviourTreeUtils.LoadBehaviourTree((TextAsset)btFile);
                if (DeserializedNode != null)
                    SetParentToNodes(DeserializedNode);
            }


            if (DeserializedNode == null)
            {
                GenericMenu toolsMenu = new GenericMenu();
                foreach (var item in BehaviourTreeUtils.GetEnumerableOfType<Node>())
                {
                    //node.type = item;
                    toolsMenu.AddItem(new GUIContent(item.Name), false, CreateRoot, item);
                }
                toolsMenu.DropDown(new Rect(lastPos + new Vector2(30, 0), new Vector2(150, 25)));
                return;
            }



            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, true, true, GUILayout.Width(this.position.width), GUILayout.Height(this.position.height));
            GUILayout.BeginHorizontal();
            AddLine();

            GUILayout.Label(new GUIContent(btFile.name));
            if (ModifiedTreeLastFrame)
                SetParentToNodes(DeserializedNode);
            PrintNode(DeserializedNode); //printing tree

            AddLine(); AddLine();


            //utilities buttons
            if (GUI.Button(new Rect(lastPos, defaultSize * 2f), new GUIContent("Change Tree (don't save changes)"))) //change tree button
            {
                btFile = null;
                DeserializedNode = null;
            }
            Tab(defaultSize.x * 2);
            if (GUI.Button(new Rect(lastPos, defaultSize * 2f), new GUIContent("Reset Tree (write on file)"))) //reset tree button
                ResetTree();
            Tab(defaultSize.x * 2);
            if (GUI.Button(new Rect(lastPos, defaultSize * 2f), new GUIContent("Save Tree (write on file)"))) //save tree button
                Save();
            GUILayout.Label("", GUILayout.Width(scrollWidth), GUILayout.Height(scrollHeight));

            GUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// Print all nodes of deserialized xml file on this editor window. Start from root node and it is a recursive call.
        /// </summary>
        /// <param name="node"></param>
        private void PrintNode(Node node)
        {
            string nameAttribute = "";
            if (node.parent == null)
                nameAttribute = "(Root) ";
            if (GUI.Button(new Rect(lastPos, defaultSize), nameAttribute + node.GetType().Name, EditorStyles.toolbarButton))
            {
                GenericMenu toolsMenu = new GenericMenu();
                toolsMenu.AddItem(new GUIContent("Delete", "Delete this node and all sub nodes"), false, Delete, node);
                foreach (var item in BehaviourTreeUtils.GetEnumerableOfType<Node>())
                {
                    if (BehaviourTreeUtils.IsCondition(item))
                    {
                        toolsMenu.AddItem(new GUIContent("Add Child/Conditions/" + item.Name), false, AddChild, new ModifiedItem(item.AssemblyQualifiedName, node));
                        toolsMenu.AddItem(new GUIContent("Change/Conditions/" + item.Name), false, ChangeNode, new ModifiedItem(item.AssemblyQualifiedName, node));
                    }
                    else if (!BehaviourTreeUtils.IsCondition(item))
                    {
                        toolsMenu.AddItem(new GUIContent("Add Child/Behaviours/" + item.Name), false, AddChild, new ModifiedItem(item.AssemblyQualifiedName, node));
                        toolsMenu.AddItem(new GUIContent("Change/Behaviours/" + item.Name), false, ChangeNode, new ModifiedItem(item.AssemblyQualifiedName, node));
                    }
                    else
                    {
                        toolsMenu.AddItem(new GUIContent("Add Child/Custom/" + item.Name), false, AddChild, new ModifiedItem(item.AssemblyQualifiedName, node));
                        toolsMenu.AddItem(new GUIContent("Change/Custom/" + item.Name), false, ChangeNode, new ModifiedItem(item.AssemblyQualifiedName, node));
                    }
                }

                toolsMenu.DropDown(new Rect(lastPos + new Vector2(30, 0), new Vector2(150, 25)));
            }
            if (node.GetChildCount() <= 0) return;
            Tab();
            foreach (var n in node.Children)
            {
                AddLine();
                PrintNode(n);
            }
            LeaveTab();
        }

        void Tab()
        {
            lastPos += new Vector2(35, 0);
            scrollWidth += 40;
        }
        void Tab(float value)
        {
            lastPos += new Vector2(value, 0);
            scrollWidth += value;
        }
        void AddLine()
        {
            lastPos += new Vector2(0, 25);
            scrollHeight += 30;
        }
        void AddLine(float value)
        {
            lastPos += new Vector2(0, value);
            scrollHeight += value;
        }
        void LeaveTab()
        {
            lastPos -= new Vector2(35, 0);
        }
        void CreateNewXmlFile(bool assignToEditor = true)
        {
            if (newFileCreatedName.Length <= 0)
            {
                Debug.LogError("Unable to create Tree. File name can't be empty.");
                return;
            }
            if (!Directory.Exists(defaultPathForSavingXmlFile))
                Directory.CreateDirectory(defaultPathForSavingXmlFile);
            if (File.Exists(defaultPathForSavingXmlFile + newFileCreatedName + ".xml"))
            {
                Debug.LogError("File alredy exist, please change file name.");
                return;
            }
            using (FileStream stream = File.Create(defaultPathForSavingXmlFile + newFileCreatedName + ".xml"))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(defaultXmlFirstLine);
                stream.Write(bytes, 0, bytes.Length);
            }
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            if (!assignToEditor) return;
            TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(defaultPathForSavingXmlFile + newFileCreatedName + ".xml");
            btFile = textAsset;
            newFileCreatedName = "";
        }
        void CreateRoot(object root)
        {
            Type t = (Type)root;
            Node rootNode = (Node)Activator.CreateInstance(t);
            DeserializedNode = rootNode;
        }
        void AddChild(object item)
        {
            ModifiedItem modItem = (ModifiedItem)item;
            Node newNode = (Node)Activator.CreateInstance(Type.GetType(modItem.newType));
            modItem.node.AddChild(newNode);
            ModifiedTreeLastFrame = true;
        }
        void Delete(object editorNode)
        {
            Node realNode = (Node)editorNode;
            if (realNode.parent == null)
            {
                //resetBT
                ResetTree();
                return;
            }
            realNode.parent.Children.Remove(realNode);
            ModifiedTreeLastFrame = true;
        }
        void ChangeNode(object item)
        {
            ModifiedItem modItem = (ModifiedItem)item;
            Node newNode = (Node)Activator.CreateInstance(Type.GetType(modItem.newType));
            //modItem.node = newNode; //not work, but why?

            int index = modItem.node.parent.Children.IndexOf(modItem.node);
            modItem.node.parent.Children[index] = newNode;
            ModifiedTreeLastFrame = true;
        }
        /// <summary>
        /// Check all Tree and set correctly the parents of all nodes
        /// </summary>
        /// <param name="node"></param>
        void SetParentToNodes(Node node)
        {
            foreach (var child in node.Children)
            {
                child.parent = node;
                if (child.Children.Count > 0)
                    SetParentToNodes(child);
            }
            ModifiedTreeLastFrame = false;
        }
        void ResetTree()
        {
            //must be better implemented (without write on file)
            DeserializedNode = null;
            using (StreamWriter writer = new StreamWriter(string.Format(defaultPathForSavingXmlFile + btFile.name + ".xml")))
                writer.WriteLine();
            Debug.Log("Tree resetted");
            AssetDatabase.Refresh();
        }
        void Save()
        {
            using (FileStream stream = new FileStream(string.Format(defaultPathForSavingXmlFile + btFile.name + ".xml"), FileMode.OpenOrCreate))
                BehaviourTreeUtils.Serializer.Serialize(stream, DeserializedNode);
            Debug.Log("Tree saved");
            AssetDatabase.Refresh();
        }

        
    }
    /// <summary>
    /// Identify an object modified in a deserialized xml file.
    /// </summary>
    public class ModifiedItem
    {
        public string newType;
        public Node node;

        public ModifiedItem(string t, Node n)
        {
            newType = t;
            node = n;
        }
    }
}

