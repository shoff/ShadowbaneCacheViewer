
#pragma once

#include "stdafx.h"
#include "model.h"
#include "skeleton.h"

class RenderInfo 
{
	Vec3D pos;	// Position

	std::vector<RenderInfo*> renderInfo; // more render IDs/objects

public:
	RenderInfo(const unsigned int fileID);
	~RenderInfo();

	inline void Draw();
	inline void DrawBounds();
	void SetSkeleton(Skeleton *s);

	wxString joint; // Name of joint that the mesh is to attach to.
	Model* mesh;	// Mesh that this render info references to
};

// Still nfi what this chunk is used for
struct unknownData1
{
	unsigned int i1;
	unsigned short s1;
	unsigned short s2;
	float posX;
	float posY;
	float posZ;
	float f1;
	float f2;
	float f3;
	float f4;
	float scaleX;
	float scaleY;
	float scaleZ;
};



struct CollisionInfo
{
	unsigned int nVectors;	// Number of of vectors per polygon
	Vec3D bounds[4];		// make it a static array of 4 for now,  if all 4 aren't used - who cares.
	
	Vec3D upVector;			// Unknown - looks like an Up-Vector, will assume it is for now
	
	unsigned short order[6]; // The order in which the vectors are to be rendered ?
	
	Vec3D unknown;			// Unknown
};


class CObject 
{
	// internal loading functions - Put this into their own functions - help clean up the constructor a bit
	void LoadType3(const unsigned char *data, unsigned int ptr); // Objects
	void LoadType4(const unsigned char *data, unsigned int ptr); // Structures
	void LoadType5(const unsigned char *data, unsigned int ptr); // Structures
	void LoadType9(const unsigned char *data, unsigned int ptr); // Items
	void LoadType13(const unsigned char *data, unsigned int ptr); // NPCs
	void LoadType15(const unsigned char *data, unsigned int ptr); // Deeds
	// Note: Could probably make a "Base Object",  then design other class objects for the different types that inherit the base object
	
	std::vector<CollisionInfo> collisionInfo;
	std::vector<RenderInfo*> renderInfo; 

	Skeleton *skeleton;

	// Some boolean values that are important but haven't figured out yet
	bool bValue1;
	bool bValue2;
	bool bValue3;
	bool bWalkData;

	uint32 renderID;	// objects render id
	uint32 invTex;		// Id of objects inventory icon texture
	uint32 mapTex;		// id of objects minimap icon texture

	// not currently used but will be needed eventually
	Vec3D scale;	// Object scale
	Vec3D pos;		// Object base position

public:
	//CObject();
	CObject(unsigned char *data);
	CObject(const unsigned int fileID);
	~CObject();

	void Create(unsigned char *data);

	void Animate();		// Animate the model - if there is any animation (skeletal, particles, etc)
	
	void DrawModel();	// Draw the model(s)
	void DrawBounds();	// Draw the Boundries
	void DrawWalkable(); // Draw the walkable area
	void DrawSkeleton(); // Draw skeleton

	Skeleton* GetSkeleton() { return skeleton; }

	wxString name;  // name of object
	uint32 flag;	// object type

	Texture *icon;	// icon for inventory/minimap
};

