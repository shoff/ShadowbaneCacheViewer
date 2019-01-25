
#include "stdafx.h"
#include "model.h"

Model::Model(unsigned int fileID)
{
	vertices = NULL;
	normals = NULL;
	texCoords = NULL;
	indices = NULL;
	tex = NULL;

	nVertices = 0;
	nNormals = 0;
	nTexCoords = 0;
	nIndices = 0;

	unsigned char *buffer = 0;
	buffer = meshArchive.getFileByID(fileID);

	if (!buffer) // huh?  Try the standard cArchive then
	{
		buffer = cArchive.getFileByID(fileID);
	}

	// error check
	if (!buffer)
	{
		return; // should probably report an error message aswell
	}

	// Create / Initiate / Setup / whatever you want to call it.
	Create(buffer);

}

Model::Model(unsigned char *buffer)
{
	vertices = NULL;
	normals = NULL;
	texCoords = NULL;
	indices = NULL;
	tex = NULL;
	meshSize = 0.0f;
	nVertices = 0;
	nNormals = 0;
	nTexCoords = 0;
	nIndices = 0;

	// error check
	if (!buffer)
		return;

	Create(buffer);
}

void Model::Create(unsigned char *data)
{
	if (!data)
		return;

	// Copy the the mesh header data
	memcpy(&header, data, sizeof(ModelHeader));

	// set the vertices for the model boundry
	SetBounds(Vec3D(header.min[0], header.min[1], header.min[2]),
		Vec3D(header.max[0], header.max[1], header.max[2]));

	// Set the pointer to 46 (where the header info finishes).
	uint32 ptr = 46;

	// Vertices
	memcpy(&nVertices, data + ptr, 4);
	ptr += 4;
	
	vBufSize = (nVertices * sizeof(float) * 3); // X Y Z

	vertices = new Vec3D[nVertices];
	memcpy(vertices, data + ptr, vBufSize);
	ptr += vBufSize;
	// --

	// Normals
	memcpy(&nNormals, data + ptr, 4);
	ptr += 4;

	nBufSize = (nNormals * sizeof(float) * 3); // X Y Z

	normals = new Vec3D[nNormals];
	memcpy(normals, data + ptr, nBufSize);
	ptr += nBufSize;
	// --

	// Texture Coordinates
	memcpy(&nTexCoords, data + ptr, 4);
	ptr += 4;

	tBufSize = (nTexCoords * sizeof(float) * 2); // U V

	texCoords = new Vec2D[nTexCoords];
	memcpy(texCoords, data + ptr, tBufSize);
	ptr += tBufSize;
	// --

	// Indices
	memcpy(&nIndices, data + ptr, 4);
	ptr += 4;

	// Experiment to see if the extra data at the bottom of mesh files is more indices
	//nIndices *= 3; // three indices per triangle maybe ?

	indices = new uint16[nIndices];
	memcpy(indices, data + ptr, (nIndices * sizeof(uint16)));
	ptr += (nIndices * sizeof(uint16));
	// --

	// Some more data at the end of the mesh - currently nfi what it is, appears to be more indices.
}


void Model::SetBounds(const Vec3D min, const Vec3D max)
{
	Vec3D size = max - min;
	meshSize = (size.x + size.y + size.z) / 3;

	// front face
	bounds[0] = max;						// Top left
	bounds[1] = Vec3D(min.x, max.y, max.z); // Top right
	bounds[2] = Vec3D(min.x, min.y, max.z); // Bottom right
	bounds[3] = Vec3D(max.z, min.y, max.z); // bottom left

	// right face
	bounds[4] = Vec3D(min.x, max.y, max.z); // top left
	bounds[5] = Vec3D(min.x, max.y, min.z); // top right
	bounds[6] = min;						// bottm right
	bounds[7] = Vec3D(min.x, min.y, max.z); // bottom left

	// back face
	bounds[8] = Vec3D(min.x, max.y, min.z);		// Top left
	bounds[9] = Vec3D(max.x, max.y, min.z);		// top right
	bounds[10] = Vec3D(max.x, min.y, min.z);	// bottom right
	bounds[11] = min;							// bottom left
	
	// left face
	bounds[12] = Vec3D(max.x, max.y, min.z);	// Top left
	bounds[13] = max;							// Top right
	bounds[14] = Vec3D(max.x, min.y, max.z);	// bottom right
	bounds[15] = Vec3D(max.x, min.y, min.z);	// bottom left
}


void Model::DrawModel()
{
	// If we have a texture, bind it
	if (tex && tex->id) {
		tex->Bind();
	} else {
		glBindTexture(GL_TEXTURE_2D, 0);
		glColor3f(0.0f, 0.8f, 0.3f);
	}

	// draw triangles
	glBegin(GL_TRIANGLES);
	for (size_t i=0; i<nIndices; i++) {
		uint16 a = indices[i];

		#ifdef _DEBUG
		if (a > nVertices || a > nTexCoords) // Error check when running in debug mode
			break;
		#endif


		glNormal3fv(normals[a]);
		glTexCoord2fv(texCoords[a]);
		glVertex3fv(vertices[a]);
	}
	glEnd();

	if (!tex || !tex->id)
		glColor3f(1.0f, 1.0f, 1.0f);
}


void Model::DrawBounds()
{
	// Set to render as lines
	glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);

	glColor3f(0.0f, 0.2f, 0.8f);

	glBegin(GL_QUADS);
		// Draw the boundry lines
		for(unsigned int i=0; i<16; i++)
			glVertex3fv(bounds[i]);

	glEnd();

	glColor3f(1.0f, 1.0f, 1.0f);

	// finished drawing, set back to default
	glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);
}


Model::~Model()
{
	wxDELETEA(vertices);
	wxDELETEA(normals);
	wxDELETEA(texCoords);
	wxDELETEA(indices);
	
	wxDELETE(tex);
}

