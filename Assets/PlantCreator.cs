using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant {
    List<Branch> branches;
    
    int depth;
    public float branch_probability;
    float branch_rotate_range;
    
    public string teststring = "test";
    
    public Plant(float length, int d, float b_prob, float b_rotate_range){
        this.depth=d;
        this.branch_probability=b_prob;
        this.branch_rotate_range=b_rotate_range;
        
        this.branches=new List<Branch>();
        this.branches.Add(new Branch(this, null, length, this.branch_rotate_range,this.depth));
    }
    
    public void DrawPlant(){
        foreach (Branch b in this.branches){
            Debug.DrawLine(b.origin, b.origin+b.vct, Color.white, 1.0f);            
            
        }
    }
    
    public void AddBranch(Branch b){
        this.branches.Add(b);
    }
}

public class Branch{
    public Vector3 vct;
    public Vector3 origin;
    
    public Quaternion RotOffset;
    
    int depth;
    
    Branch parent;
    List<Branch> children = new List<Branch>();
    
    public Branch(Plant plant, Branch prnt, float length, float rotate_range, int d){
        
        this.depth=d;
                
        this.RotOffset=Quaternion.Euler(Random.Range(-rotate_range,rotate_range),
                                    0,
                                    Random.Range(-rotate_range,rotate_range));
        
        this.parent=prnt;
        if(prnt==null){
            this.vct=RotOffset*(Vector3.up * length); //set these to defaults since this is the root
            this.origin = Vector3.zero;
        }
        else{
            this.vct=prnt.RotOffset*RotOffset*(Vector3.up * length); //multiply by parent's rotation
            this.origin=prnt.origin+prnt.vct;  //offset by parent's endpoint
        }
        
        bool FirstBranch = true;
        
        if(this.depth>0){
            while( (Random.Range(0.0f,100.0f)/100.0f < plant.branch_probability) || FirstBranch){
                FirstBranch=false;
                Branch b = new Branch(plant, this, length*0.8f, rotate_range,this.depth-1);
                this.children.Add(b);
                plant.AddBranch(b);
            }
        }
        
        
    }
}

[System.Serializable]
public class PlantCreator : MonoBehaviour {

    Plant p;
    
    
    public int PlantDepth=5;
    public float BranchingProbability=.2f;
    public float BranchRotationRange=15;
    public float BranchLength=1;
    

	void Start () {
		p = new Plant(
            BranchLength,
            PlantDepth,
            BranchingProbability,
            BranchRotationRange
        );
        
        
        
	}
	
	void Update () {
		p.DrawPlant();
	}
}
