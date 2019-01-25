
#pragma once

#include "stdafx.h"
#include "cachelib.h"

class Texture 
{
private:
	
public:

	// Constructor (__ctor) and destructor
	Texture(unsigned char *data, unsigned int size);
	Texture(unsigned int fileID);
	Texture();
	~Texture();

	void Create(unsigned char *data, unsigned int size);

	// OpenGL funcs
	void Bind();
	void Unbind();

	// Other functions
	void SaveToFile(const wxChar *fn);

	// Opengl Texture ID
	GLuint id;

	// Width x Height x Bitdepth
	unsigned int width;
	unsigned int height;
	int bytes;

	// Image Data Buffer
	unsigned int bufSize;
	unsigned char *buffer;

};

