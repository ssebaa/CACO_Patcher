using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Skyrim.Records;
using Noggog;
using static Mutagen.Bethesda.Skyrim.Furniture;
using Mutagen.Bethesda.Strings;
using DynamicData;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace CACO_Patcher.Utilities {
    public static class RecordUtilities {
        /// <summary>
        /// Merges scripts from a VM adapter into winning override.
        /// </summary>
        /// <param name="adapterToMerge">The adapter to merge</param>
        /// <param name="baseRecord">The first occurrence (earliest in the load-order) of the object we are patching</param>
        /// <param name="patchAdapter">The adapter to merge into</param>
        /// <returns>
        /// modified VirtualMachineAdapter, or null.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static VirtualMachineAdapter? MergeVMAdapters(IVirtualMachineAdapterGetter? adapterToMerge, IHaveVirtualMachineAdapterGetter baseRecord, VirtualMachineAdapter? patchAdapter) {
            //merge scripts
            if (adapterToMerge is null) {
                System.Console.WriteLine("No scripts to merge");
                return patchAdapter;
            }
            if (patchAdapter is null) {
                patchAdapter = adapterToMerge.DeepCopy();
                System.Console.WriteLine("overriding null scripts");
                return patchAdapter;
            }
            //add records that aren't in base or patch into patch
            ExtendedList<ScriptEntry> patchRecordsCopy = new(patchAdapter.Scripts);
            Dictionary<String, IScriptEntryGetter> toMergeFormKeyCache = new();
            Dictionary<String, IScriptEntryGetter> baseRecordFormKeyCache = new();
            Dictionary<String, ScriptEntry> patchRecordFormKeyCache = new();
            foreach (var entry in adapterToMerge.Scripts) {
                toMergeFormKeyCache.Add(entry.Name, entry);
            }
            foreach (var entry in baseRecord.VirtualMachineAdapter is null ? new ExtendedList<IScriptEntryGetter>() : baseRecord.VirtualMachineAdapter.Scripts) {
                baseRecordFormKeyCache.Add(entry.Name, entry);
            }
            foreach (var entry in patchAdapter.Scripts) {
                patchRecordFormKeyCache.Add(entry.Name, entry);
            }
            foreach (var record in adapterToMerge.Scripts) {
                if (!baseRecordFormKeyCache.TryGetValue(record.Name, out var baseRecordScript)) {
                    patchRecordsCopy.Add(record.DeepCopy());
                    continue;
                }
                if (baseRecordScript.Properties.Equals(record.Properties)) {
                    continue;
                }
                patchRecordFormKeyCache.TryGetValue(record.Name, out var patchScriptRecord);
                baseRecordFormKeyCache.TryGetValue(record.Name, out var baseScriptRecord);
                patchScriptRecord?.Properties.SetTo(MergeScriptProperties(record.Properties, baseScriptRecord?.Properties ?? new ExtendedList<IScriptPropertyGetter>(), patchScriptRecord?.Properties ?? new ExtendedList<ScriptProperty>()));
            }
            //remove records that toMerge removes
            List<ScriptEntry> toRemove = new();
            foreach (var record in baseRecord.VirtualMachineAdapter is null ? new ExtendedList<IScriptEntryGetter>() : baseRecord.VirtualMachineAdapter.Scripts) {
                if (!toMergeFormKeyCache.ContainsKey(record.Name)) {
                    foreach (var patchRecord in patchAdapter.Scripts) {
                        if (patchRecord.Name.Equals(record.Name)) {
                            toRemove.Add(patchRecord);
                            break;
                        }
                    }
                }
            }
            foreach (var record in toRemove) {
                patchRecordsCopy.Remove(record);
            }
            patchAdapter.Scripts.SetTo(patchRecordsCopy);
            return patchAdapter;
        }
        public static ExtendedList<ScriptProperty> MergeScriptProperties(IReadOnlyList<IScriptPropertyGetter> propertiesToMerge, IReadOnlyList<IScriptPropertyGetter> baseProperties, ExtendedList<ScriptProperty> patchProperties) {
            //add properties that aren't in base or patch into patch
            var patchPropertiesCopy = new ExtendedList<ScriptProperty>(patchProperties);
            foreach (IScriptPropertyGetter property in propertiesToMerge) {
                if (baseProperties.Contains(property) || patchProperties.Contains(property)) {
                    continue;
                }
                patchPropertiesCopy.Add(property.DeepCopy());
            }
            //remove properties that propertiesToMerge removes
            foreach (IScriptPropertyGetter property in baseProperties) {
                if (propertiesToMerge.Contains(property)) {
                    continue;
                }
                patchPropertiesCopy.Remove(property.DeepCopy());
            }
            return patchPropertiesCopy;
        }
        /// <summary>
        /// Merges scripts from a quest VM adapter into winning override.
        /// </summary>
        /// <param name="adapterToMerge">The adapter to merge</param>
        /// <param name="baseRecord">The first occurrence (earliest in the load-order) of the object we are patching</param>
        /// <param name="patchAdapter">The adapter to merge into</param>
        /// <returns>
        /// modified QuestAdapter, or null.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static QuestAdapter? MergeQuestVMAdapters(IQuestAdapterGetter? adapterToMerge, IHaveVirtualMachineAdapterGetter baseRecord, QuestAdapter? patchAdapter) {
            //merge scripts
            if (adapterToMerge is null) {
                System.Console.WriteLine("No scripts to merge");
                return patchAdapter;
            }
            if (patchAdapter is null) {
                patchAdapter = adapterToMerge.DeepCopy();
                System.Console.WriteLine("overriding null scripts");
                return patchAdapter;
            }
            //add records that aren't in base or patch into patch
            ExtendedList<ScriptEntry> patchRecordsCopy = new(patchAdapter.Scripts);
            Dictionary<String, IScriptEntryGetter> toMergeFormKeyCache = new();
            Dictionary<String, IScriptEntryGetter> baseRecordFormKeyCache = new();
            Dictionary<String, ScriptEntry> patchRecordFormKeyCache = new();
            foreach (var entry in adapterToMerge.Scripts) {
                toMergeFormKeyCache.Add(entry.Name, entry);
            }
            foreach (var entry in baseRecord.VirtualMachineAdapter is null ? new ExtendedList<IScriptEntryGetter>() : baseRecord.VirtualMachineAdapter.Scripts) {
                baseRecordFormKeyCache.Add(entry.Name, entry);
            }
            foreach (var entry in patchAdapter.Scripts) {
                patchRecordFormKeyCache.Add(entry.Name, entry);
            }
            foreach (var record in adapterToMerge.Scripts) {
                if (!baseRecordFormKeyCache.TryGetValue(record.Name, out var baseRecordScript)) {
                    patchRecordsCopy.Add(record.DeepCopy());
                    continue;
                }
                if (baseRecordScript.Properties.Equals(record.Properties)) {
                    continue;
                }
                patchRecordFormKeyCache.TryGetValue(record.Name, out var patchScriptRecord);
                baseRecordFormKeyCache.TryGetValue(record.Name, out var baseScriptRecord);
                patchScriptRecord?.Properties.SetTo(MergeScriptProperties(record.Properties, baseScriptRecord?.Properties ?? new ExtendedList<IScriptPropertyGetter>(), patchScriptRecord?.Properties ?? new ExtendedList<ScriptProperty>()));
            }
            //remove records that toMerge removes
            List<ScriptEntry> toRemove = new();
            foreach (var record in baseRecord.VirtualMachineAdapter is null ? new ExtendedList<IScriptEntryGetter>() : baseRecord.VirtualMachineAdapter.Scripts) {
                if (!toMergeFormKeyCache.ContainsKey(record.Name)) {
                    foreach (var patchRecord in patchAdapter.Scripts) {
                        if (patchRecord.Name.Equals(record.Name)) {
                            toRemove.Add(patchRecord);
                            break;
                        }
                    }
                }
            }
            foreach (var record in toRemove) {
                patchRecordsCopy.Remove(record);
            }
            patchAdapter.Scripts.SetTo(patchRecordsCopy);
            return patchAdapter;
        }
        public static List<MessageButton> MergeMessageButtons(IReadOnlyList<IMessageButtonGetter> toMerge, IReadOnlyList<IMessageButtonGetter> baseRecords, ExtendedList<MessageButton> patchRecords) {
            ExtendedList<MessageButton> patchRecordsCopy = new(patchRecords);
            //add records that aren't in base or patch into patch
            foreach (var record in toMerge) {
                if (baseRecords.Contains(record) || patchRecords.Contains(record)) {
                    continue;
                }
                patchRecordsCopy.Add(record.DeepCopy());
            }
            //remove records that toMerge removes
            foreach (var record in baseRecords) {
                if (toMerge.Contains(record)) {
                    continue;
                }
                patchRecordsCopy.Remove(record.DeepCopy());
            }
            return patchRecordsCopy;
        }
        /// <summary>
        /// Merges lists of IFormLinkGetter.
        /// </summary>
        /// <param name="linksToMerge">The list to merge</param>
        /// <param name="baseRecordLinks">The list in the first occurrence (earliest in the load-order) of the object we are patching</param>
        /// <param name="patchLinks">The list to merge into</param>
        /// <returns>
        /// ExtendedList<IFormLinkGetter<IMajorRecordGetter>>?
        /// </returns>
        /// <remarks>
        /// Input and output both require extensive casting to be of any use, recommend to use helper function instead of calling this general-case function
        /// example for patching Keywords in an Ingestible:
        /// var merged = MergeFormLinkList(keywordsToMerge ?? new ExtendedList<IFormLinkGetter<IKeywordGetter>>(), baseRecordKeywords ?? new ExtendedList<IFormLinkGetter<IKeywordGetter>>(), patchKeywords.Cast<IFormLinkGetter<IMajorRecordGetter>>().ToExtendedList())?.Cast<IFormLinkGetter<IKeywordGetter>>().ToExtendedList() ?? patchKeywords;
        /// 
        /// </remarks>
        public static ExtendedList<IFormLinkGetter<IMajorRecordGetter>> MergeFormLinkList(IReadOnlyList<IFormLinkGetter<IMajorRecordGetter>> toMerge, IReadOnlyList<IFormLinkGetter<IMajorRecordGetter>> baseRecords, ExtendedList<IFormLinkGetter<IMajorRecordGetter>> patchRecords) {
            //add records that aren't in base or patch into patch
            foreach (var record in toMerge) {
                if (baseRecords.Contains(record) || patchRecords.Contains(record)) {
                    continue;
                }
                patchRecords.Add(record);
            }
            //remove records that toMerge removes
            foreach (var record in baseRecords) {
                if (toMerge.Contains(record)) {
                    continue;
                }
                patchRecords.Remove(record);
            }
            return patchRecords;
        }
        public static ExtendedList<ContainerEntry> MergeContainerEntryList(IReadOnlyList<IContainerEntryGetter>? toMerge, IReadOnlyList<IContainerEntryGetter>? baseRecords, ExtendedList<ContainerEntry> patchRecords) {
            if (toMerge is null) {
                return patchRecords ?? new();
            }
            baseRecords ??= new ExtendedList<IContainerEntryGetter>();
            //add records that aren't in base or patch into patch
            ExtendedList<ContainerEntry> patchRecordsCopy = new(patchRecords);
            Dictionary<FormKey, IContainerEntryGetter> toMergeFormKeyCache = new();
            Dictionary<FormKey, IContainerEntryGetter> baseRecordFormKeyCache = new();
            Dictionary<FormKey, IContainerEntryGetter> patchRecordFormKeyCache = new();
            foreach (var entry in toMerge) {
                toMergeFormKeyCache.Add(entry.Item.Item.FormKey, entry);
            }
            foreach (var entry in baseRecords) {
                baseRecordFormKeyCache.Add(entry.Item.Item.FormKey, entry);
            }
            foreach (var entry in patchRecords) {
                patchRecordFormKeyCache.Add(entry.Item.Item.FormKey, entry);
            }
            foreach (var record in toMerge) {
                if (!baseRecordFormKeyCache.TryGetValue(record.Item.Item.FormKey, out var baseRecord)) {
                    patchRecordsCopy.Add(record.DeepCopy());
                    continue;
                }
                if (baseRecord.Item.Count == record.Item.Count) {
                    continue;
                }
                foreach (var patchRecord in patchRecordsCopy) {
                    if (patchRecord.Item.Item.FormKey.Equals(record.Item.Item.FormKey)) {
                        //merge counts
                        int a = (patchRecord.Item.Count - baseRecord.Item.Count);
                        int b = (record.Item.Count - baseRecord.Item.Count);
                        if ((a ^ b) >= 0) {
                            patchRecord.Item.Count = baseRecord.Item.Count + a + Math.Abs(a - b) / 2;
                        }
                        else {
                            patchRecord.Item.Count = baseRecord.Item.Count + a + b;
                        }
                        break;
                    }
                }
            }
            //remove records that toMerge removes
            List<ContainerEntry> toRemove = new();
            foreach (var record in baseRecords) {
                if (!toMergeFormKeyCache.ContainsKey(record.Item.Item.FormKey)) {
                    foreach (var patchRecord in patchRecords) {
                        if (patchRecord.Item.Item.FormKey.Equals(record.Item.Item.FormKey)) {
                            toRemove.Add(patchRecord);
                            break;
                        }
                    }
                }
            }
            foreach (var record in toRemove) {
                patchRecordsCopy.Remove(record);
            }
            return patchRecordsCopy;
        }
        /// <summary>
        /// Merges lists of IKeywordGetter.
        /// </summary>
        /// <param name="keywordsToMerge">The list to merge</param>
        /// <param name="baseRecordKeywords">The list in the first occurrence (earliest in the load-order) of the object we are patching</param>
        /// <param name="patchKeywords">The list to merge into</param>
        /// <returns>
        /// ExtendedList<IFormLinkGetter<IKeywordGetter>>?
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static ExtendedList<IFormLinkGetter<IKeywordGetter>> MergeKeywordList(IReadOnlyList<IFormLinkGetter<IKeywordGetter>>? keywordsToMerge, IReadOnlyList<IFormLinkGetter<IKeywordGetter>>? baseRecordKeywords, ExtendedList<IFormLinkGetter<IKeywordGetter>> patchKeywords) {
            return MergeFormLinkList(keywordsToMerge ?? new ExtendedList<IFormLinkGetter<IKeywordGetter>>(), baseRecordKeywords ?? new ExtendedList<IFormLinkGetter<IKeywordGetter>>(), patchKeywords.Cast<IFormLinkGetter<IMajorRecordGetter>>().ToExtendedList())?.Cast<IFormLinkGetter<IKeywordGetter>>().ToExtendedList() ?? patchKeywords;

        }
        //Why did I write this?
        //The world may never know
        //but it could be useful so I'll keep it.
        public static IMajorRecordGetter GetLastOccurrenceOfRecordInModList(IMajorRecordGetter recordToFind, IEnumerable<ISkyrimGroupGetter<ISkyrimMajorRecordGetter>> ListOfRecordLists) {
            ListOfRecordLists.Reverse();
            foreach (ISkyrimGroupGetter<ISkyrimMajorRecordGetter> recordList in ListOfRecordLists) {
                foreach (ISkyrimMajorRecordGetter record in recordList) {
                    if (record.FormKey.Equals(recordToFind.FormKey)) {
                        return record;
                    }
                }
            }
            return recordToFind;
        }
        /// <summary>
        /// Returns the earliest occurrence of this record.
        /// </summary>
        /// <param name="formKeyToFind">The FormKey we are looking for</param>
        /// <param name="ListOfModMasterRecordLists">The list of IMajorRecordGetters from the mods listed as masters of the mod being processed</param>
        /// <param name="thisModRecordList">The list of IMajorRecordGetters from the mod being processed</param>
        /// <returns>
        /// (IMajorRecordGetter?, bool)
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static (IMajorRecordGetter, bool) GetBaseRecord(FormKey formKeyToFind, IEnumerable<ISkyrimGroupGetter<ISkyrimMajorRecordGetter>?> ListOfModMasterRecordLists, ISkyrimGroupGetter<ISkyrimMajorRecordGetter> thisModRecordList) {
            foreach (ISkyrimGroupGetter<ISkyrimMajorRecordGetter>? recordList in ListOfModMasterRecordLists) {
                if (recordList is not null) {
                    foreach (ISkyrimMajorRecordGetter record in recordList) {
                        if (record.FormKey.Equals(formKeyToFind)) {
                            return (record, true);
                        }
                    }
                }
            }
            foreach (ISkyrimMajorRecordGetter record in thisModRecordList) {
                if (record.FormKey.Equals(formKeyToFind)) {
                    return (record, false);
                }
            }
            //this should never happen
            System.Console.Error.WriteLine("FormKey passed to getBaseRecord was not found in any passed lists- make sure they are the correct lists");
            throw new InvalidDataException();
        }
    }
}
