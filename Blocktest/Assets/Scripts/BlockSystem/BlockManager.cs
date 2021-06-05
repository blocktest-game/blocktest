// System required for [Serializable] attribute.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BlockSystem
{
    public class BlockManager : MonoBehaviour
    {

        /// <summary> Array which stores all block instances for referencing as if they were Globals. </summary>
        public Block[] allBlocks;
        /// <summary> Dictionary which stores the instances of blocks as values with their types as keys. </summary>
        public readonly Dictionary<Type, Block> blockDict = new Dictionary<Type, Block>();
        /// <summary> List used to store the names of blocks. The indexes are the corresponding block's ID. </summary>
        [HideInInspector] public string[] blockNames;

        /// <summary> Dropdown used for player item selection </summary>
        [SerializeField] private Dropdown selectionDropdown;

        private void Awake()
        {
            // This mess gets all subtypes of Block and puts the types in a list.
            Type[] allBlockTypes = (
                from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                from assemblyType in domainAssembly.GetTypes()
                where assemblyType.IsSubclassOf(typeof(Block))
                select assemblyType).ToArray();

            allBlocks = new Block[allBlockTypes.Length];
            blockNames = new string[allBlockTypes.Length];

            // For loops to populate main allBlocks array.
            for (int i = 0; i < allBlockTypes.Length; i++) {
                Type newBlockType = allBlockTypes[i];
                Block newBlock = (Block)Activator.CreateInstance(newBlockType);
                newBlock.Initialize();
                if (newBlock.blockID == -1) {
                    newBlock.blockID = i;
                }
                if (allBlocks[newBlock.blockID] is { }) {
                    Debug.LogWarning("Block " + newBlock + " conflicts with block " + allBlocks[newBlock.blockID] + "! (Block ID: " + newBlock.blockID + ")");
                } else if (newBlock.blockID > allBlocks.Length || newBlock.blockID < 0) {
                    Debug.LogWarning("Block " + newBlock + " has invalid ID " + newBlock.blockID + "! (Max ID " + allBlocks.Length + ")");
                }
                blockNames[newBlock.blockID] = newBlock.blockName;
                allBlocks[newBlock.blockID] = newBlock;
                blockDict.Add(newBlockType, newBlock);
            }
            selectionDropdown.AddOptions(blockNames.ToList());
            
            WorldGen.GenerateMainMap(); // TODO: This should be in globals or something, but it needs to be called AFTER the block manager is loaded.
        }

        // Code band aid for saving
        public void PlayerLoadLevel() {
            SaveSystem.LoadGame(0);
        }

        public void PlayerSaveLevel() {
            SaveSystem.SaveGame(0);
        }

        // disgusting
        public void ChangeBlockSelection(int slot) => Globals.characterObject.GetComponent<PlayerUI>().ChangeBlockSelection(slot);
    }
}
