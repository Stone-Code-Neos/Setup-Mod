//using System;
//using System.Collections.Generic;
//using System.Linq;
//using HarmonyLib;
//using NeosModLoader;
//using FrooxEngine;
//using FrooxEngine.LogiX.Tools;
//using FrooxEngine.UIX;
//using BaseX;
//using FrooxEngine.CommonAvatar;
//using FrooxEngine.FinalIK;
//using FrooxEngine.LogiX.ProgramFlow;

//namespace NoTankControls
//{
//    public class XyMod : NeosMod
//    {
//        public override string Name => "TF4";
//        public override string Author => "dann";
//        public override string Version => "1.0.0";


//        public override void OnEngineInit()
//        {
//            Harmony harmony = new Harmony("U-dann.StatueUtilities");
//            harmony.PatchAll();
//        }

//        // [HarmonyPatch(typeof(DynamicImpulseReceiverWithValue<IAssetProvider<Material>>))]
//        // [HarmonyPatch("Trigger")]
//        class EncasementMaterialPatch
//        {
//            private static void Postfix(DynamicImpulseReceiverWithValue<IAssetProvider<Material>> __instance,
//                AssetProvider<Material> value)
//            {
//                __instance.Debug.Log(value.Name);
//            }
//        }

//        // [HarmonyPatch(typeof(DynamicImpulseReceiverWithValue<string>))]
//        // [HarmonyPatch("Trigger")]
//        class Patch
//        {
//            private static void Postfix(DynamicImpulseReceiverWithValue<string> __instance, string value)
//            {
//                // string tag = __instance.Tag.Evaluate();
//                // __instance.Debug.LogText(tag);
//                // __instance.Debug.LogText(value);
//                //
//                // if (tag == "StatueUtilities/Encasement")
//                // {
//                //     __instance.Debug.LogText("StatueUtilities/Encasement");
//                //     
//                //     if (value == "GenerateTableMaterial")
//                //     {
//                //         __instance.Debug.LogText("GenerateTableMaterial");
//                //         Slot logixSlot = __instance.Slot.Parent.Parent;
//                //         Slot materialSlot = logixSlot.FindInChildren("varMaterial");
//                //         IAssetProvider<Material> material = materialSlot.GetComponent<ReferenceField<IAssetProvider<Material>>>().Reference.Target;
//                //         PBS_DistanceLerpMetallic distanceLerp = materialSlot.AttachComponent<PBS_DistanceLerpMetallic>();
//                //         MaterialHelper.CopyMaterialProperties(material, distanceLerp);
//                //     }
//                // }
//                if (__instance.Tag.Evaluate() == "TF3")
//                {
//                    if (value == "Setup") // Set up the material slots on the avatar
//                    {s
//                        __instance.Debug.LogText("TF4: Starting");

//                        try
//                        {
//                            // Top Level Slots
//                            Slot avatarSlot;
//                            Slot statuficationSystemSlot;
//                            Slot statuficationSystemAddonsSlot;

//                            // Setup Slots
//                            Slot volumeNormalSlot;
//                            Slot volumeWhisperSlot;
//                            Slot smoothTransformSpeedSlot;
//                            Slot smoothTransformActiveSlot;
//                            Slot disableOnStatueSlot;
//                            Slot materialLerpSlot;
//                            Slot bodyNormalActiveSlot;
//                            Slot bodyStatueActiveSlot;
//                            Slot statueMaterialSlotAsignerSlot;
//                            Slot noCutoutSystemSlot;

//                            // Bodies
//                            Slot bodyNormalSlot;
//                            Slot bodyStatueSlot;
//                            Slot armatureSlot;
//                            List<Slot> armatureStatueSlots = new List<Slot>();
//                            List<Slot> armatureNormalSlots = new List<Slot>();

//                            statuficationSystemSlot = __instance.Slot;

//                            __instance.Debug.LogText("TF4: Got initial slot");

//                            while (statuficationSystemSlot.Name != "<color=#dadada>Statuefication</color>")
//                            {
//                                statuficationSystemSlot = statuficationSystemSlot.Parent;
//                            }

//                            __instance.Debug.LogText("TF4: Got statue system slot");

//                            bool FindArmature(Slot child)
//                            {
//                                return child.Name == "Armature";
//                            }

//                            //Initial Setup
//                            avatarSlot = statuficationSystemSlot.Parent.Parent;
//                            armatureSlot = avatarSlot.FindChild(FindArmature);
//                            statuficationSystemSlot = avatarSlot.FindInChildren("<color=#dadada>Statuefication</color>");
//                            statuficationSystemAddonsSlot = avatarSlot.FindInChildren("<color=#dadada>Statue Add-Ons</color>");

//                            List<SkinnedMeshRenderer> armatureMeshRenderers =
//                                armatureSlot.GetComponentsInChildren<SkinnedMeshRenderer>();

//                            foreach (SkinnedMeshRenderer skinnedMeshRenderer in armatureMeshRenderers)
//                            {
//                                armatureNormalSlots.Add(skinnedMeshRenderer.Slot);
//                                Slot statueSlot = skinnedMeshRenderer.Slot.Duplicate();
//                                armatureStatueSlots.Add(statueSlot);
//                                skinnedMeshRenderer.Slot.Name = skinnedMeshRenderer.Slot.Name + " Normal";
//                                statueSlot.Name = statueSlot.Name + " Statue";
//                            }

//                            __instance.Debug.LogText("TF4: Got avatar slot and addons slot");

//                            // Volume - Normal+
//                            volumeNormalSlot = statuficationSystemSlot.FindInChildren("Volume - Normal+");
//                            ValueMultiDriver<float> volumeNormalMultiDriver =
//                                volumeNormalSlot.GetComponentInChildren<ValueMultiDriver<float>>();
//                            AvatarAudioOutputManager audioOutputManager = avatarSlot.GetComponentInChildren<AvatarAudioOutputManager>();

//                            __instance.Debug.LogText("TF4: Got volume slot");

//                            volumeNormalMultiDriver.Drives[0].ForceLink(audioOutputManager.NormalConfig.Volume);
//                            volumeNormalMultiDriver.Drives[1].ForceLink(audioOutputManager.ShoutConfig.Volume);
//                            volumeNormalMultiDriver.Drives[2].ForceLink(audioOutputManager.BroadcastConfig.Volume);

//                            __instance.Debug.LogText("TF4: Driving volumes normal");

//                            // Volume - Whisper
//                            volumeWhisperSlot = statuficationSystemSlot.FindInChildren("Volume - Whisper");
//                            ValueMultiDriver<float> volumeWhisperMultiDriver =
//                                volumeWhisperSlot.GetComponentInChildren<ValueMultiDriver<float>>();

//                            volumeWhisperMultiDriver.Drives[0].ForceLink(audioOutputManager.WhisperConfig.Volume);

//                            __instance.Debug.LogText("TF4: Driving volume whisper");

//                            // Smooth Transform
//                            Slot rightHandProxySlot = avatarSlot.FindInChildren("Right Hand Proxy");
//                            Slot leftHandProxySlot = avatarSlot.FindInChildren("Left Hand Proxy");
//                            Slot rightHandTargetSlot = rightHandProxySlot.FindInChildren("Target");
//                            Slot leftHandTargetSlot = leftHandProxySlot.FindInChildren("Target");

//                            __instance.Debug.LogText("TF4: Found hand proxy and targets");

//                            SmoothTransform rightHandSmoothTransform = rightHandTargetSlot.AttachComponent<SmoothTransform>();
//                            SmoothTransform leftHandSmoothTransform = leftHandTargetSlot.AttachComponent<SmoothTransform>();

//                            // Smooth Transform - Speed
//                            smoothTransformSpeedSlot = statuficationSystemSlot.FindInChildren("Smooth Transform - Speed");
//                            ValueMultiDriver<float> smoothTransformSpeedValueMultiDriver =
//                                smoothTransformSpeedSlot.GetComponentInChildren<ValueMultiDriver<float>>();

//                            smoothTransformSpeedValueMultiDriver.Drives[0].ForceLink(rightHandSmoothTransform.SmoothSpeed);
//                            smoothTransformSpeedValueMultiDriver.Drives[1].ForceLink(leftHandSmoothTransform.SmoothSpeed);

//                            // Smooth Transform - Active
//                            smoothTransformActiveSlot = statuficationSystemSlot.FindInChildren("Smooth Transform - Active");
//                            ValueMultiDriver<bool> smoothTransformActiveValueMultiDriver =
//                                smoothTransformActiveSlot.GetComponentInChildren<ValueMultiDriver<bool>>();

//                            smoothTransformActiveValueMultiDriver.Drives[0].ForceLink(rightHandSmoothTransform.EnabledField);
//                            smoothTransformActiveValueMultiDriver.Drives[1].ForceLink(leftHandSmoothTransform.EnabledField);

//                            __instance.Debug.LogText("TF4: Set up smooth transforms");

//                            // Disable on Statue
//                            disableOnStatueSlot = statuficationSystemSlot.FindInChildren("Disable on Statue");
//                            ValueMultiDriver<bool> disableOnStatueValueMultiDriver =
//                                disableOnStatueSlot.GetComponentInChildren<ValueMultiDriver<bool>>();

//                            VRIK vRIK = avatarSlot.GetComponentInChildren<VRIK>();
//                            VisemeAnalyzer visemeAnalyzer = avatarSlot.GetComponentInChildren<VisemeAnalyzer>();
//                            List<DynamicBoneChain> dynamicBoneChains = avatarSlot.GetComponentsInChildren<DynamicBoneChain>();
//                            List<HandPoser> handPosers = avatarSlot.GetComponentsInChildren<HandPoser>();
//                            EyeManager eyeManager = avatarSlot.GetComponentInChildren<EyeManager>();

//                            disableOnStatueValueMultiDriver.Drives[1].ForceLink(vRIK.EnabledField);
//                            disableOnStatueValueMultiDriver.Drives[2].ForceLink(visemeAnalyzer.EnabledField);
//                            disableOnStatueValueMultiDriver.Drives[3].ForceLink(eyeManager.EnabledField);
//                            int disableOnStatueIndex = 4;

//                            foreach (DynamicBoneChain dynamicBoneChain in dynamicBoneChains)
//                            {
//                                disableOnStatueValueMultiDriver.Drives.Add();
//                                disableOnStatueValueMultiDriver.Drives[disableOnStatueIndex].ForceLink(dynamicBoneChain.EnabledField);
//                                disableOnStatueIndex++;
//                            }

//                            foreach (HandPoser handPoser in handPosers)
//                            {
//                                disableOnStatueValueMultiDriver.Drives.Add();
//                                disableOnStatueValueMultiDriver.Drives[disableOnStatueIndex].ForceLink(handPoser.EnabledField);
//                                disableOnStatueIndex++;
//                            }

//                            __instance.Debug.LogText("TF4: Set up disable on statue");

//                            // Body Active
//                            bodyNormalActiveSlot = statuficationSystemSlot.FindInChildren("Body Normal Active");
//                            bodyStatueActiveSlot = statuficationSystemSlot.FindInChildren("Body Statue Active");
//                            ValueMultiDriver<bool> bodyNormalActiveValueMultiDriver =
//                                bodyNormalActiveSlot.GetComponentInChildren<ValueMultiDriver<bool>>();
//                            ValueMultiDriver<bool> bodyStatueActiveValueMultiDriver =
//                                bodyStatueActiveSlot.GetComponentInChildren<ValueMultiDriver<bool>>();


//                            bodyNormalSlot = avatarSlot.FindInChildren("Body");
//                            bodyStatueSlot = bodyNormalSlot.Duplicate();

//                            bodyNormalSlot.NameField.ForceSet("Body Normal");
//                            bodyStatueSlot.NameField.ForceSet("Body Statue");

//                            bodyNormalActiveValueMultiDriver.Drives[0].ForceLink(bodyNormalSlot.ActiveSelf_Field);
//                            bodyStatueActiveValueMultiDriver.Drives[0].ForceLink(bodyStatueSlot.ActiveSelf_Field);

//                            int slotDriveIndex = 1;

//                            foreach (Slot normalSlot in armatureNormalSlots)
//                            {
//                                bodyNormalActiveValueMultiDriver.Drives.Add();
//                                bodyNormalActiveValueMultiDriver.Drives[slotDriveIndex].Target =
//                                    normalSlot.ActiveSelf_Field;
//                                slotDriveIndex++;
//                            }

//                            slotDriveIndex = 1;

//                            foreach (Slot statueSlot in armatureStatueSlots)
//                            {
//                                bodyStatueActiveValueMultiDriver.Drives.Add();
//                                bodyStatueActiveValueMultiDriver.Drives[slotDriveIndex].Target =
//                                    statueSlot.ActiveSelf_Field;
//                                slotDriveIndex++;
//                            }

//                            __instance.Debug.LogText("TF4: Set up body active");

//                            // Statue Material Slot Assigner
//                            statueMaterialSlotAsignerSlot = statuficationSystemSlot.FindInChildren("Statue Material Slot Asigner");

//                            ReferenceMultiDriver<IAssetProvider<Material>> statueMaterialSlotAsignerValueMultiDriver =
//                                statueMaterialSlotAsignerSlot.GetComponentInChildren<ReferenceMultiDriver<IAssetProvider<Material>>>();

//                            List<SkinnedMeshRenderer> statueMeshRenderers =
//                                bodyStatueSlot.GetComponentsInChildren<SkinnedMeshRenderer>();

//                            PBS_Metallic statueMaterial = bodyStatueSlot.AttachComponent<PBS_Metallic>();
//                            statueMaterialSlotAsignerValueMultiDriver.Reference.Target = statueMaterial;

//                            int drivesIndex = 0;

//                            foreach (SkinnedMeshRenderer skinnedMeshRenderer in statueMeshRenderers)
//                            {
//                                for (int i = 0; i < skinnedMeshRenderer.Materials.Count; i++)
//                                {
//                                    statueMaterialSlotAsignerValueMultiDriver.Drives.Add();
//                                    statueMaterialSlotAsignerValueMultiDriver.Drives[drivesIndex].ForceLink(skinnedMeshRenderer.Materials.GetElement(i));
//                                    drivesIndex++;
//                                }
//                            }

//                            statueMaterialSlotAsignerValueMultiDriver.Drives.EnsureExactCount(drivesIndex);

//                            __instance.Debug.LogText("TF4: Set up material slot assigner");

//                            // No Cutout System
//                            noCutoutSystemSlot = statuficationSystemSlot.FindInChildren("No Cutout System");
//                            ValueField<BlendMode> blendModeValue = noCutoutSystemSlot.GetComponentInChildren<ValueField<BlendMode>>();
//                            Sync<BlendMode> blendModeField = (Sync<BlendMode>)(statueMaterialSlotAsignerValueMultiDriver
//                                .Drives[0]
//                                .Target.Target.TryGetField("BlendMode"));
//                            blendModeValue.Value.Value = blendModeField.Value;

//                            ValueMultiDriver<BlendMode> blendModeValueMultiDriver =
//                                noCutoutSystemSlot.GetComponentInChildren<ValueMultiDriver<BlendMode>>();

//                            ValueMultiDriver<color> colorValueMultiDriver = noCutoutSystemSlot.GetComponentInChildren<ValueMultiDriver<color>>();

//                            List<SkinnedMeshRenderer> bodyMeshRenderers =
//                                bodyNormalSlot.GetComponentsInChildren<SkinnedMeshRenderer>();

//                            int materialIndex = 0;

//                            foreach (SkinnedMeshRenderer bodyMeshRenderer in bodyMeshRenderers)
//                            {
//                                for (int i = 0; i < bodyMeshRenderer.Materials.Count; i++)
//                                {
//                                    if (bodyMeshRenderer.Materials[i] != null)
//                                    {
//                                        blendModeValueMultiDriver.Drives.Add();
//                                        colorValueMultiDriver.Drives.Add();

//                                        Sync<BlendMode> bodyBlendModeField;
//                                        Sync<color> albedoColorField;

//                                        try
//                                        {
//                                            bodyBlendModeField =
//                                                (Sync<BlendMode>)(bodyMeshRenderer.Materials[i]
//                                                    .TryGetField("BlendMode"));

//                                            blendModeValueMultiDriver.Drives[materialIndex].ForceLink(bodyBlendModeField);
//                                        }
//                                        catch (Exception e)
//                                        {
//                                            __instance.Debug.LogText("TF3: Could not get blend mode field");
//                                        }

//                                        try
//                                        {
//                                            albedoColorField =
//                                                (Sync<color>)(bodyMeshRenderer.Materials[i]
//                                                    .TryGetField("AlbedoColor"));
//                                            colorValueMultiDriver.Drives[materialIndex].ForceLink(albedoColorField);
//                                        }
//                                        catch (Exception e)
//                                        {
//                                            albedoColorField =
//                                                (Sync<color>)(bodyMeshRenderer.Materials[i]
//                                                    .TryGetField("Color"));
//                                            colorValueMultiDriver.Drives[materialIndex].ForceLink(albedoColorField);
//                                        }

//                                        materialIndex++;
//                                        __instance.Debug.LogText(bodyMeshRenderer.Materials[i].Name);
//                                    }
//                                }
//                            }
//                            __instance.Debug.LogText("Set up no cutout system");
//                        }
//                        catch (Exception e)
//                        {
//                            __instance.Debug.Error(e);
//                        }

//                    }
//                }
//            }
//        }
//    }
//}
