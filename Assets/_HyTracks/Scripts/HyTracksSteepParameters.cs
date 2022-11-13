using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HyTracks
{
    [CreateAssetMenu(menuName = "HyTracks/SteepParameters")]
    public class HyTracksSteepParameters:ScriptableObject {
        public DateTime date;

        [Header("Input Parameters Text Files")]
        public TextAsset parametersSocialInputJSON;
        public TextAsset parametersTechnologyInputJSON;        
        public TextAsset parametersEconomicsInputJSON;
        public TextAsset parametersEnvironmentInputJSON;
        public TextAsset parametersPoliticsInputJSON;

        [Header("Output Parameters Text Files")]
        public TextAsset parametersSocialOutputJSON;
        public TextAsset parametersTechnologyOutputJSON;
        public TextAsset parametersEconomicsOutputJSON;
        public TextAsset parametersEnvironmentOutputJSON;
        public TextAsset parametersPoliticsOutputJSON;

        

        [Header("SOCIAL")]
        [Space(10)]
        public HyTracksParametersList parametersSocialInput;
        public HyTracksParametersList parametersSocialOutput;
        [Space(10)]
        [Header("TECHNOLOGY")]
        public HyTracksParametersList parametersTechnologyInput;
        public HyTracksParametersList parametersTechnologyOutput;
        [Space(10)]
        [Header("ECONOMICS")]
        public HyTracksParametersList parametersEconomicsInput;
        public HyTracksParametersList parametersEconomicsOutput;
        [Space(10)]
        [Header("ENVIRONMENT")]
        public HyTracksParametersList parametersEnvironmentInput;
        public HyTracksParametersList parametersEnvironmentOutput;
        [Space(10)]
        [Header("POLITICS")]
        public HyTracksParametersList parametersPoliticsInput;
        public HyTracksParametersList parametersPoliticsOutput;

        public Dictionary<STEEPDimension,HyTracksParametersList> parametersInput { private set; get; }
        public Dictionary<STEEPDimension,HyTracksParametersList> parametersOutput { private set; get; }

        [EditorCools.Button]
        void GenerateParameterDicts() {
            parametersInput = new Dictionary<STEEPDimension,HyTracksParametersList>() {
                {STEEPDimension.SOCIAL, parametersSocialInput },
                {STEEPDimension.TECHNOLOGY, parametersTechnologyInput},
                {STEEPDimension.ECONOMICS, parametersEconomicsInput},
                {STEEPDimension.ENVIRONMENT, parametersEnvironmentInput},
                {STEEPDimension.POLITICS, parametersPoliticsInput},
            };

            parametersOutput = new Dictionary<STEEPDimension,HyTracksParametersList>() {
                {STEEPDimension.SOCIAL, parametersSocialOutput},
                {STEEPDimension.TECHNOLOGY, parametersTechnologyOutput},
                {STEEPDimension.ECONOMICS, parametersEconomicsOutput},
                {STEEPDimension.ENVIRONMENT, parametersEnvironmentOutput},
                {STEEPDimension.POLITICS, parametersPoliticsOutput},
            };
        }
        

        private void OnEnable()
        {
            LoadParametersFromFiles();
            GenerateParameterDicts();
        }


        [EditorCools.Button]
        void LoadParametersFromFiles()
        {
            if(parametersSocialInputJSON != null) {
                parametersSocialInput = JsonUtility.FromJson<HyTracksParametersList>(parametersSocialInputJSON.text);
            }
            if(parametersTechnologyInputJSON != null) {
                parametersTechnologyInput = JsonUtility.FromJson<HyTracksParametersList>(parametersTechnologyInputJSON.text);
            }
            if(parametersEconomicsInputJSON != null) {
                parametersEconomicsInput = JsonUtility.FromJson<HyTracksParametersList>(parametersEconomicsInputJSON.text);
            }
            if(parametersEnvironmentInputJSON != null) {
                parametersEnvironmentInput = JsonUtility.FromJson<HyTracksParametersList>(parametersEnvironmentInputJSON.text);
            }
            if(parametersPoliticsInputJSON != null) {
                parametersPoliticsInput = JsonUtility.FromJson<HyTracksParametersList>(parametersPoliticsInputJSON.text);
            }
        }
    }
}
