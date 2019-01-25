
#pragma once

#include "vec3d.h"
#include "vec4d.h"
#include "Texture.h"
#include "cachelib.h"


//50 byte header
// 46th byte of the header is the vertex count
// following the header (immediately after the vertex count
// is VertexCount * 4 (float) * 3 (Vector)
// At the end of this chunk is another count (which always seems to be the same as the one in the header)

struct ModelHeader {
	uint32 null1;
	uint32 Unk2;
	uint32 Unk3;
	uint32 Unk4;
	uint32 Unk5;
	float min[3];
	float max[3];
	uint16 null2;
};

class Model
{
private:
	ModelHeader header;

	Vec3D bounds[16];

	float meshSize;

	Vec3D *vertices;
	Vec3D *normals;
	Vec2D *texCoords;
	uint16 *indices;

	int32 nVertices;
	uint32 nNormals;
	uint32 nTexCoords;
	uint32 nIndices;

	uint32 vBufSize;
	uint32 nBufSize;
	uint32 tBufSize;

public:

	Model(unsigned char *buffer);
	Model(unsigned int fileID);
	~Model();

	void Create(unsigned char *data);
	void SetBounds(const Vec3D min, const Vec3D max);

	void DrawModel();
	void DrawBounds();

	// Objects
	Texture *tex;
};



