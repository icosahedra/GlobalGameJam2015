using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Icosahedra{
public class AnimationController : MonoBehaviour {

	[SerializeField] private new Animation animation;

	private List<AnimationStateData> stateData = new List<AnimationStateData>();
	private Dictionary<AnimationNode, AnimationStateData> animationLookup = new Dictionary<AnimationNode, AnimationStateData>();


	private const float SYNC_TOLERANCE = 0.1f;

	public void AddAnimation(AnimationNode d){
	
		if(!animationLookup.ContainsKey(d) ){
			//Debug.Log(d + " : " + d.name + " : " + d.Clip);
			
			if(animation[d.name] != null){
				Debug.LogError("Cannot attach clip twice" + d.name);	
				return;
			}
			if(d.Clip == null){
				Debug.LogError( "Clip is null in object: " + d.name);	
				return;
			}
			animation.AddClip(d.Clip, d.name);
			
			AnimationState s = animation[d.name];
		
		
			if(s != null){
				AnimationStateData data = new AnimationStateData(s, d);
				stateData.Add(data);
				animationLookup.Add(d,data);
			}
		}
		
	}

	private void Update(){
		
		for(int i=0; i<stateData.Count; i++){
			AnimationStateData sData = stateData[i];
			
			if(sData.State.weight == 0 && sData.State.wrapMode == WrapMode.Once){  
				// A hack for 'Once' mode, prevents a "once" animation from continually blending in
				sData.targetWeight =0;
			}
			
			//If a state has not reached its target weight
			if(sData.State.weight != sData.targetWeight){
				
				float deltaWeight = (sData.targetWeight - sData.State.weight);
				float maxBlendDistance = sData.blendSpeed*Time.deltaTime; //maximum allowed distance
				
				if(Mathf.Abs(deltaWeight) > maxBlendDistance){  //If weight greater than max, clamp for linear blend
					sData.State.weight = sData.State.weight + Mathf.Sign(deltaWeight)*maxBlendDistance;
//					Debug.Log(sData.State.weight + " : " + sData.State + ": "+ sData.targetWeight);
				}
				else{  
					//You've reached the end of the blend - assign the value directly
					sData.State.weight = sData.targetWeight;
//					Debug.Log(sData.State.weight + " done: " + sData.State + ": "+ sData.targetWeight);
				}
			}
			if(sData.State.weight == 0  && sData.targetWeight == 0){
				sData.State.enabled = false;
				//Debug.Log(sData.State.name);
			}
				
		}
		

	}

	public bool IsPlaying(AnimationNode d){
		if(d != null){
			if(!animationLookup.ContainsKey(d)){
				AddAnimation(d);
			}
			AnimationStateData sData = animationLookup[d];
			return sData.State.enabled;
		}
		
		//Debug.LogWarning("AnimationNode is null, Equipment is probably misconfigured");
		return false;
	}
	public float Weight(AnimationNode d){
		if(d != null){
			if(!animationLookup.ContainsKey(d)){
				AddAnimation(d);
			}
			AnimationStateData sData = animationLookup[d];
			return sData.State.weight;
		}
		
		//Debug.LogWarning("AnimationNode is null, Equipment is probably misconfigured");
		return 0;
	}
	
	//Plays the animation from the beginning, resetting speed values to normal
	public void PlayAnimation(AnimationNode d){
		if(d != null){
			if(!animationLookup.ContainsKey(d)){
				AddAnimation(d);
			}
			
			AnimationStateData sData = animationLookup[d];
			sData.State.speed = sData.DefaultSpeed;
			
			StopAnimationsByLayer(sData.State.layer); //Stop everything on this layer
		
			sData.targetWeight = 1; //set target weight to 1
			
			sData.State.time = 0.0f; //Set the time to 0
			sData.State.weight = 1; //Put the clip to full weight
			sData.State.enabled = true; //turn the animation on
		}
		else{
			//Debug.LogWarning("AnimationNode is null, Equipment is probably misconfigured");
		}
	}
	
	public void StopAnimation(AnimationNode d){
		if(d != null){
			if(!animationLookup.ContainsKey(d)){
				AddAnimation(d);
			}
			
			AnimationStateData sData = animationLookup[d];
			sData.targetWeight = 0; //set target weight to 0
			
			sData.State.time = 0.0f; //Set the time to 0
			sData.State.weight = 0; //turn the clip weight to 0
			sData.State.enabled = false; //turn the animation on
	//			desc.autoFade = false;
		}
		else{
			//Debug.LogWarning("AnimationNode is null, Equipment is probably misconfigured");
		}
	}
	
	public float GetNormalizedTime(AnimationNode d)
	{
		if (d == null) return 0;
		if(!animationLookup.ContainsKey(d)){
			AddAnimation(d);
		}
		AnimationStateData sData = animationLookup[d];
		return sData.State.normalizedTime;
		
	}
	
	public void SetNormalizedTime(AnimationNode d, float normalizedTime)
	{
		if (d == null) return;
		if(!animationLookup.ContainsKey(d)){
			AddAnimation(d);
		}
		AnimationStateData sData = animationLookup[d];
		sData.State.normalizedTime = normalizedTime;
		sData.State.enabled = true;
	}
	
	public void BlendAnimation(AnimationNode d, float targetWeight, float blendSpeed){
		if(d != null){
			if(!animationLookup.ContainsKey(d)){
				AddAnimation(d);
			}
			AnimationStateData sData = animationLookup[d];
			
	//		desc.autoFade = false;
	//		sData.State.speed = sData.DefaultSpeed;
			if(sData.targetWeight != targetWeight){  //also check blendSpeed?
				//This is a hack to get their once mode to work//
				if(sData.State.weight == 0 && sData.State.wrapMode == WrapMode.Once){
					sData.State.weight = 0.01f;
				}
				
				//We want all other animations that are playing to fade out at the speed this one fades in//
	//				KillTargetWeightsByLayer(desc.state.layer, blendSpeed);
				
				sData.targetWeight = targetWeight;
				sData.blendSpeed = blendSpeed;
				sData.State.enabled = true;
			}
			else if(sData.targetWeight > 0){
				sData.State.enabled = true;
			}
			
		}
		else{
			//Debug.LogWarning("AnimationNode is null, Equipment is probably misconfigured");
		}
	}
	
	public void SetBlendWeight(AnimationNode d, float targetWeight){
		if(d != null){
			if(!animationLookup.ContainsKey(d)){
				AddAnimation(d);
			}
			AnimationStateData sData = animationLookup[d];
				
			sData.targetWeight = targetWeight;
			sData.State.enabled = true;
			sData.State.weight = sData.targetWeight;
	//		Debug.Log(sData.State.weight);
		}
		else{
			//Debug.LogWarning("AnimationNode is null, Equipment is probably misconfigured");
		}
	}
	public void SetPlaybackSpeed(AnimationNode d, float targetSpeed){
		if(d != null){
			if(!animationLookup.ContainsKey(d)){
				AddAnimation(d);
			}
			AnimationStateData sData = animationLookup[d];
				
			sData.State.speed = sData.DefaultSpeed*targetSpeed;
	//		Debug.Log(sData.State.weight);
		}
		else{
			//Debug.LogWarning("AnimationNode is null, Equipment is probably misconfigured");
		}
	}
	
	public void StopAnimationsByLayer(int layer){   // immediately stop all animations on a layer
		for(int i = 0; i<stateData.Count; i++){
			AnimationStateData sData = stateData[i];
			
			if(sData.State.layer == layer){
				sData.targetWeight = 0;
				sData.State.enabled = false;	
				sData.State.weight = 0;	
				sData.State.time = 0.0f;
				
			}
		}
	}
	
	///blend out all animations on a single layer
	public void BlendOutLayer(int layer, float speed){// Fade out all animations on a layer at a certain speed
		for(int i = 0; i<stateData.Count; i++){
			AnimationStateData sData = stateData[i];
			
			if(sData.State.layer == layer){  
				sData.targetWeight = 0;
				sData.blendSpeed = speed;
			}
		}
	}
	
	///immediately stop every animation
	public void StopAll(){ 
		for(int i = 0; i<stateData.Count; i++){
			AnimationStateData sData = stateData[i];
			
			sData.State.time = 0.0f;
			sData.State.enabled = false;	
			sData.State.weight = 0;	
			sData.targetWeight = 0;
		}
	}
	///blend out all animations
	private void BlendOutAll(float speed){// Fade out all animations on a layer at a certain speed
		for(int i = 0; i<stateData.Count; i++){
			AnimationStateData sData = stateData[i];
			
			
			sData.targetWeight = 0;
			sData.blendSpeed = speed;
		}
	}
	
	public float SyncAnimations(AnimationNode from, AnimationNode to, float weight, float blendSpeed, float masterWeight=1){
		if(!animationLookup.ContainsKey(from)){
			AddAnimation(from);
		}
		if(!animationLookup.ContainsKey(to)){
			AddAnimation(to);
		}
		
			
		AnimationStateData sFrom = animationLookup[from];
		AnimationStateData sTo = animationLookup[to];
		
		float normalizedTime = sTo.State.normalizedTime;
		
		float syncDelta = sFrom.State.normalizedTime - sTo.State.normalizedTime;
		if(Mathf.Abs(syncDelta) > SYNC_TOLERANCE){
			//Debug.Log("bah Sync - " + sTo.State.normalizedTime + " : " + sFrom.State.normalizedTime + " : " + Time.time);
			
			
			//We always advance time, never rewind...
			if(sFrom.State.normalizedTime > sTo.State.normalizedTime){
//				Debug.Log("time back");
				normalizedTime = sTo.State.normalizedTime = sFrom.State.normalizedTime;
			}
			else{
//				Debug.Log("time forward");
				normalizedTime = sFrom.State.normalizedTime = sTo.State.normalizedTime;
			}
//			sTo.State.normalizedTime += Mathf.Clamp01(5*Time.deltaTime)*syncDelta;
			
		}

		sFrom.targetWeight = masterWeight*(1-weight);
		sTo.targetWeight = masterWeight*(weight);
		
		float w1Norm = (1-weight);
		float w2Norm = (weight);
		
		sFrom.blendSpeed = blendSpeed;
		sTo.blendSpeed = blendSpeed;
		
		float factor2 = sTo.NormalizedLength / (sFrom.NormalizedLength * (w1Norm) +sTo.NormalizedLength * w2Norm);
		float ratio = sFrom.NormalizedLength / sTo.NormalizedLength;
		
		sTo.State.speed = sTo.DefaultSpeed * factor2;
		sFrom.State.speed = sFrom.DefaultSpeed * factor2 * ratio;
		
		sFrom.State.enabled = true;
		sTo.State.enabled = true;
			
		return normalizedTime;

	}




}
[System.Serializable]
	public class AnimationStateData{
		public AnimationStateData( AnimationState state, AnimationNode s) {
			 this.state = state; 
			 sourceData = s;
			 Initialize();
		}
	
		private void Initialize(){
			state.blendMode = sourceData.BlendMode;
			state.layer = sourceData.Layer;
			
			defaultSpeed = sourceData.Speed;	
			normalizedLength = state.length / defaultSpeed;
			state.speed = defaultSpeed;
			state.wrapMode = sourceData.WrapMode;
			state.weight = 0;	
		}
	
	
		private AnimationNode sourceData;
		
		
		private AnimationState state;
		public AnimationState State{
			get{ return state; }
		}	
		
		public float targetWeight;
		public float blendSpeed;	
//		public bool autoFade = false; //hmm, this is an odd one
		
		private float defaultSpeed;
		public float DefaultSpeed{
			get{ return defaultSpeed; }	
		}
		
		private float normalizedLength; //Length / Speed
		public float NormalizedLength{
			get{ return normalizedLength; }	
		}
	}

}
