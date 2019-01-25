
#pragma once

#include "vec3d.h"
#include "vec4d.h"
#include "stdafx.h"
#include "matrix.h"
#include "model.h"


struct AnimBlock
{
	Vec3D pos;
	Vec4D rot;
	Vec3D scale;
};


class Bone {
public:
	Bone();
	~Bone();

	// Reads in the bone data from the buffer, and returns how many bytes in read
	unsigned int Init(const unsigned char *data);
	void Setup(std::vector<Bone> *allbones, Bone *p);

	// animate
	void CalcMatrix(const unsigned int frame);

	// Draw bone segment
	inline void DrawBone();

	// Draw mesh connected to bone
	inline void DrawMesh();
	

	int32 id;			// joint ID, some bones have -1 for ID
	wxString name;		// name of joint

	Model *mesh;		// mesh attached to the bone, if any.

	// HIERARCHY
	uint32 nChildren;	// Number of child nodes/bones;
	Bone *parent;
	Bone **children;


	// BASE DATA
	Vec3D dir;		// Direction of bone (rotation)
	Vec3D axis;			// axis for rotation?
	float length;		// length of bone
	
	// Transformed data
	Vec3D pos;		// Position of the bone
	Vec4D rot;		// transformed rotation
	Vec3D scale;	// scale
	Vec3D r;


	Matrix mat;			// local matrix for this segment ?
	Matrix mrot;		// local rotation matrix?

	// MISC DATA
	bool flip;			// Flip the mesh?  (invert (-1) scale)
	bool setup;			// false, set to true once its been setup.
	
	// Animation data for current animation
	std::vector<AnimBlock*> animBlocks;
};

class Skeleton {
private:

	// Skeleton Name
	wxString name;

	Vec3D scale;

	// BONES
	int32 nBones;
	std::vector<Bone> bones;

	// ANIMATION
	unsigned int nFrames;	// number of frames
	unsigned int curFrame;	// current frame of the animation the skeleton is upto
	unsigned int nAnimBones; // number of bones affected by the animation

public:
	Skeleton(const unsigned int fileID);
	Skeleton(const unsigned char *data, const unsigned int dataSize);
	~Skeleton();

	void Create(const unsigned char *data, const unsigned int dataSize);
	
	Bone* GetBone(const wxString bone);

	void SetAnimation(unsigned int fileID);
	void SetScale(const Vec3D s) {scale = s;}
	void SetMesh(wxString boneName, Model *mesh);

	void Animate();

	void DrawSkeleton();
	void DrawModel();
	
	std::vector<int> animations;	// List of animations available for this skeleton
};



