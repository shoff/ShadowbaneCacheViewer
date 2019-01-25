
#include "stdafx.h"
#include "texture.h"

Texture::Texture()
{
	buffer = NULL;
	id = 0;
	width = 0;
	height = 0;
	bytes = 0;
}

Texture::Texture(unsigned char *data, unsigned int size)
{
	buffer = NULL;
	id = 0;
	width = 0;
	height = 0;
	bytes = 0;

	Create(data, size);	
}

Texture::Texture(unsigned int fileID)
{
	buffer = NULL;
	id = 0;
	width = 0;
	height = 0;
	bytes = 0;

	// ----
	unsigned char *bufPtr = 0;
	unsigned int size = 0;
	bufPtr = texArchive.getFileByID(fileID);
	size = texArchive.getFileSizeByID(fileID);

	if (!bufPtr) { // huh?  Try the standard cArchive then
		buffer = cArchive.getFileByID(fileID);
		size = cArchive.getFileSizeByID(fileID);
	}

	// error check
	if (!bufPtr)
		return; // should probably report an error message aswell

	Create(bufPtr, size);	
	

}

void Texture::Create(unsigned char *data, unsigned int size)
{
	if (!data || size==0)
		return;

	if (size>12) {
		memcpy(&width, data, 4);
		memcpy(&height, data + 4, 4);
		memcpy(&bytes, data + 8, 4);
	}

	// Get the image data
	bufSize = (size - 26);
	buffer = new unsigned char[bufSize];
	memcpy(buffer, data+26, bufSize);

	GLenum format;
	if (bytes == 1)
		format = GL_LUMINANCE;
	else if (bytes == 4)
		format = GL_RGBA;
	else
		format = GL_RGB;

	// Create the texture 
	glGenTextures(1, &id);
	glBindTexture(GL_TEXTURE_2D, id);
	glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, format, GL_UNSIGNED_BYTE, buffer);
	//glTexImage2D(GL_TEXTURE_2D, 0, bytes, width, height, 0, format, GL_UNSIGNED_BYTE, buffer);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
}

void Texture::Bind()
{
	glBindTexture(GL_TEXTURE_2D, id);
}

void Texture::Unbind()
{
	glBindTexture(GL_TEXTURE_2D, 0);
}

void Texture::SaveToFile(const wxChar *fn)
{
	if (!buffer)
		return;

	CxImage *img = new CxImage(0);
	if (img->IsEnabled())
		img->CreateFromArray(buffer, width, height, bytes*8, (width*bytes), false);
	if (img->IsValid()) {

		// Save as Jpeg
		//if (bytes > 3)
		//	img->DecreaseBpp(bytes*8, false);
		if (!img->Save(fn, CXIMAGE_FORMAT_BMP))
			wxMessageBox(_T("Failed to save the texture."), _T("Error"));

		// Save a PNG
		//img->Save(fn, CXIMAGE_FORMAT_PNG);
	}
	img->Clear();
	img->Destroy();
}



Texture::~Texture()
{
	if (id)
		glDeleteTextures(1, &id);

	wxDELETEA(buffer);
}

