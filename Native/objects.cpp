
#include "objects.h"


RenderInfo::RenderInfo(const unsigned int fileID)
{
	unsigned char *buffer = 0;
	unsigned int fileSize = 0;
	unsigned int ptr = 0;

	mesh = 0;

	unsigned int iCounter = 0;
	unsigned char letter = 0;
	unsigned int id = 0;
	unsigned char cCounter = 0;

	fileSize = renderArchive.getFileSizeByID(fileID);
	if (fileSize < 39)
	{
		wxLogMessage(_T("Could not load the ID #%i from render.cache"), fileID);
		return;
	}

	// copy the data directly into a separate buffer so that can open multiple files from the render.cache archive
	if (renderArchive.getFileByID(fileID)) 
	{
		buffer = new unsigned char[fileSize];
		memcpy(buffer, renderArchive.getFileByID(fileID), fileSize); 
	} 
	else 
	{
		wxLogMessage(_T("Could not load the ID #%i from render.cache"), fileID);
		return;
	}

	// Basic error handler
	try 
	{
		ptr = 35; // offset to mesh counter
		memcpy(&iCounter, buffer+ptr, 4);
		ptr += 4;

		// Temporary test to find out if there are ever more than 1 mesh per render file.
		if (iCounter>1)
		{
			return;
		}
		//for (unsigned int i=0; i<iCounter; i++)  {
		if (iCounter > 0) 
		{
			// I don't think its actually a counter, but a "boolean value" true/false
			ptr += 4; // skip over null int
			memcpy(&id, buffer+ptr, 4);
			ptr += 6; // read mesh id, and skip over null short

			wxLogMessage(_T("Mesh ID: %i"), id);
			mesh = new Model(id);

			// Joint name to attach to - if any.
			// -----------------
			unsigned int size = 0;
			joint = ""; // Clear

			memcpy(&size, buffer+ptr, 4);
			ptr += 4;
			
			for (unsigned int x=0; x<size; x++)
			{
				memcpy(&letter, buffer + ptr + (x*2), 1);
				joint.Append(letter);
			}

			if (!joint.IsEmpty())
			{
				wxLogMessage(_T("Joint Slot: %s"), joint.c_str());
			}

			ptr += (size * 2);
			// ^^^^^^^^^^^^^^^^
		}


		// object scale ?
		//memcpy(&scale, buffer+ptr, sizeof(Vec3D));
		ptr += sizeof(Vec3D);

		// skip over more unknown data
		ptr += 4; // skip over null int

		// object position ?
		memcpy(&pos, buffer+ptr, sizeof(Vec3D));
		ptr += sizeof(Vec3D);

		// Render object count
		memcpy(&iCounter, buffer+ptr, 4);
		ptr += 4;

		for (unsigned int i=0; i<iCounter; i++) 
		{
			ptr += 4; // skip over null bytes
			memcpy(&id, buffer+ptr, 4);
			ptr += 4; //

			wxLogMessage(_T("Render ID: %i"), id);

			// Push the new render file onto the stack
			renderInfo.push_back(new RenderInfo(id));
		}

		memcpy(&cCounter, buffer+ptr, 1);
		ptr++;

		//for (unsigned char i=0; i<cCounter; i++) {
		if (cCounter > 0) 
		{
			ptr += 12; // skip over unknown data to the texture id

			memcpy(&id, buffer + ptr, 4);
			ptr += 4;

			wxLogMessage(_T("Texture ID: %i"), id);

			if (mesh)
			{
				mesh->tex = new Texture(id);
			}

			ptr += 21; // skip over more unknown data

			/*
				dword value; // 1
				dword value; // 1
				dword value; // null
				dword texID; // texture id for mesh
				dword value; // 6
				dword value; // 0100 0001
				dword value; // null
				dword value; // null
				dword value; // 255
				dword value; // null
				word value; // 0001
			*/
		}

		/*
		0, dword value; // 1
		4, word value; // null
		6, dword weirdValue; //4704e33a
		10, dword value; // 1
		14, dword value; // null
		18, dword value; // 1
		22, dword value; // null
		36, dword value; // null
		30, dword weirdvalue; //0ad7233c

		31, byte boolval;  // 0/1
		
		<done>

		<done>
		dword value; // 1
		float x; //
		float y; 
		float z;
		
		<done>
		
		<done>

		// End of file
		// 01 01 00 00 00 00 00
		byte value; // 1
		dword value; // 1
		word value; // null
		*/
	} catch (...) {
		// error
	}

	// Clear the buffer
	wxDELETEA(buffer);
}

inline void RenderInfo::Draw()
{
	glPushMatrix();

	if (mesh)
	{
		glTranslatef(pos.x, pos.y, pos.z);
		mesh->DrawModel();
	}

	for (size_t i=0; i<renderInfo.size(); i++) 
	{
		renderInfo[i]->Draw();
	}

	glPopMatrix();
}

inline void RenderInfo::DrawBounds()
{
	if (mesh)
	{
		mesh->DrawBounds();
	}
	for (size_t i=0; i<renderInfo.size(); i++)
	{
		renderInfo[i]->DrawBounds();
	}	
}

RenderInfo::~RenderInfo()
{
	wxDELETE(mesh);

	renderInfo.clear();
}


CObject::CObject(unsigned char *data)
{
	Cremate(data);
}

CObject::CObject(const unsigned int fileID)
{
	unsigned char *data = objArchive.getFileByID(fileID);

	// Error check
	if (!data)
	{		
		return;
	}

	Create(data);
}


void CObject::Create(unsigned char *data)
{
	wxLogMessage(_T("In CObject::Create data length?: %i"), &data);
	// Initiate vars
	skeleton = 0;
	icon = 0;

	scale = Vec3D(1,1,1);
	bValue1 = bValue2 = bValue3 = bWalkData = false;
	renderID = invTex = mapTex = 0;

	// Basic error check
	if (!data)
	{
		return;
	}

	unsigned int counter = 0;
	unsigned int ptr = 4; // offset pointer to the data in the buffer

	// Error handler
	try 
	{
		// Get the flag/type info
		memcpy(&flag, data + ptr, 4);
		ptr += 4; // 8

		// Get the name, if any
		unsigned int size = 0;
		unsigned char letter = 0;

		memcpy(&size, data + ptr, 4);
		ptr += 4; // 12

		name = ""; // Clear the name
		for(unsigned int i=0; i<size; i++) 
		{
			memcpy(&letter, data + ptr + (i*2), 1);
			name.Append(letter);
		}
		// is the name of the model ... ie Centaur 
		wxLogMessage(_T("In CObject::Create letter: %s"), name.c_str());

		ptr += (size * 2);
		// ----
		// Skip over this data that we currently have no interest in
		ptr += 25;


	} catch (...) {
		// What the? - log error and exit function
		wxLogMessage(_T("Error occured in: %s, %s, Line #%i."), __FILE__, __FUNCTION__, __LINE__);
		return;
	}

	/*
	// Flag Info:
	// 0x01 = Sun
	// 0x03 = Basic objects,  Pillars, rocks, trees, monuments, static structures
	// 0x4 & 0x5 = Buildings and Interactive objects - don't know exact difference between 4 and 5.
	// 0x9 = Items, all weapons, armours, equipment, etc.
	// 0xD = All Runes / Creatures / NPCs / Characters
	// 0xF = All Deeds
	// 0x10 = Keys, Warrants, etc
	// 0x13 = Particles, Environment, Interface
	*/
	wxLogMessage(_T("Obj: %s, Type: 0x%x"), name.c_str(), flag);

	// Static objects
	if (flag == 3) 
	{
		LoadType3(data, ptr);
	// Static structures
	} else if (flag == 4) {
    	LoadType4(data, ptr);
	// Interactive Structures
	} else if (flag == 5) {
		LoadType5(data, ptr);
	// Items
	} else if (flag == 9) {
		LoadType9(data, ptr);
	// Runes / Creatures / NPC's / Characters
	} else if (flag == 13) { // 0x0D
		LoadType13(data, ptr);
	// Deeds
	} else if (flag == 15) { // 0x0F
		LoadType15(data, ptr);
	} else {
		// do nothing
	}
	
	if (mapTex)
	{
		icon = new Texture(mapTex);
	}
	else if (invTex)
	{
		icon = new Texture(invTex);
	}
}

// Type 3's are basic mesh and texture objects.
void CObject::LoadType3(const unsigned char *data, unsigned int ptr)
{
	wxLogMessage(_T("ptr in LoadType3 initially set to : %i"), ptr);

	memcpy(&renderID, data + ptr, 4);
	ptr += 4;
	
	if (renderID)		
	{
		renderInfo.push_back(new RenderInfo(renderID)); // Add the object render pass onto the stack
	}
}


// Type 4 and 5 are a more complex version of type 3,  with multiple mesh and textures that need to be positioned.
void CObject::LoadType4(const unsigned char *data, unsigned int ptr)
{	
	unsigned int iUnk = 0;
	//unknownData1 unkData1;
	unsigned int counter = 0;
	CollisionInfo collisionData;
	unsigned int renderID = 0;
	unsigned int bytesOfZeroData = 0;
	wxLogMessage(_T("ptr in LoadType4 initially set to : %i"), ptr);

	// Error handler
	try {

		while(renderID==0)
		{
			memcpy(&renderID, data + ptr, 4); // world texture id.0
			ptr += 4;
			bytesOfZeroData+=4;
		}

		wxLogMessage(_T("Encountered %i bytes of data before we reached a non-zero number!!!!"), bytesOfZeroData-4);
		wxLogMessage(_T("Render ID: %i"), renderID);
		memcpy(&invTex, data + ptr, 4); // inventory texture id
		ptr += 4;
		wxLogMessage(_T("Inventory Texture ID: %i"), invTex);

		memcpy(&mapTex, data + ptr, 4); // Get the minimap texture id
		ptr += 4;
		wxLogMessage(_T("Minimap Texture ID: %i"), mapTex);

		memcpy(&iUnk, data + ptr, 4);
		ptr += 4;
		wxLogMessage(_T("Unknown ID: %i"), iUnk);

		memcpy(&iUnk, data + ptr, 4); 
		ptr += 4;
		wxLogMessage(_T("Unknown ID: %i"), iUnk);

		// Counter for number of records of unknown data
		memcpy(&counter, data + ptr, 4);
		ptr += 4;

		// range error check
		if (counter > 100000) // one million, nothing should have more than that
		{
			throw;
		}
		// Unknown data chunks
		// These chunks contain info like position, rotation, and scale - I think.
		/* Uncomment this to check out the data in the chunks
		for (unsigned int i=0; i<counter; i++) {
			memcpy(&unkData1, data + ptr, sizeof(unknownData1));
			ptr += sizeof(unknownData1);
		}
		*/
		ptr += sizeof(unknownData1) * counter;


		// skip over unknown data
		ptr += 108; 

		// 4 bytes that seem to contain boolean info
		memcpy(&bValue1, data+ptr, 1);
		ptr++;
		memcpy(&bValue2, data+ptr, 1);
		ptr++;
		memcpy(&bValue3, data+ptr, 1);
		ptr++;
		memcpy(&bWalkData, data+ptr, 1);
		ptr++;

		// skip over more unknown data
		ptr += 7;

		// range check and if statement - some type 5 objects don't have any of this data - must be a boolean value somewhere !?
		// possible in the above 119 bytes
		if (bWalkData) 
		{
			// Counter
			memcpy(&counter, data + ptr, 4);
			ptr += 4;

			// range error check
			if (counter > 10000) // nothing should be more than that
			{
				throw;
			}

			// unknown data chunk(s)
			// These chunks contain information like colision detection, walkable areas, etc - and some other data im unsure of.
			for (unsigned int i=0; i<counter; i++) 
			{
				// Error handler
				try
				{
					memcpy(&collisionData.nVectors, data + ptr, 4); 
					ptr += 4;

					memcpy(&collisionData.bounds, data + ptr, (sizeof(Vec3D) * collisionData.nVectors));
					ptr += (sizeof(Vec3D) * collisionData.nVectors);
					
					memcpy(&collisionData.upVector, data + ptr, sizeof(Vec3D));
					ptr += sizeof(Vec3D);

					memcpy(&collisionData.order, data + ptr, sizeof(uint16) * 6);
					ptr += sizeof(uint16) * 6;
					
					memcpy(&collisionData.unknown, data + ptr, sizeof(Vec3D));
					ptr += sizeof(Vec3D);

					collisionInfo.push_back(collisionData);

				} catch (...) {
					throw;
				}
			}

			// Counter for another data chunk similar to above
			memcpy(&counter, data + ptr, 4);
			ptr += 4;

			// Error check
			if (counter < 1000)
			{
				// anything not within this range is probably bad
				unsigned int tempCounter = counter;
				for (unsigned int j=0; j<tempCounter; j++) 
				{					
					try 
					{
						// real counter to the number of chunks
						memcpy(&counter, data + ptr, 4); // Counter for the data chunk, identical to the above data chunk
						ptr += 4;

						// range error check
						if (counter > 10000) // nothing should be more than that
						{
							return;
						}
					} catch (...) {
						return;
					}

					// unknown data chunk(s)
					for (unsigned int i=0; i<counter; i++) 
					{
						// Error handler
						try 
						{

							// The "number of vectors" basically determines if its rendering as quad or a triangle
							memcpy(&collisionData.nVectors, data + ptr, 4); 
							ptr += 4;

							// Vertex data for above vectors
							memcpy(&collisionData.bounds, data + ptr, (sizeof(Vec3D) * collisionData.nVectors));
							ptr += (sizeof(Vec3D) * collisionData.nVectors);
							
							memcpy(&collisionData.upVector, data + ptr, sizeof(Vec3D));
							ptr += sizeof(Vec3D);

							memcpy(&collisionData.order, data + ptr, sizeof(uint16) * 6);
							ptr += sizeof(uint16) * 6;
							
							// Some weird anomaly for "Orc Hall" - hopefully this is a workaround
							if (collisionData.order[4] > 255)
							{
								ptr+=2;
							}

							memcpy(&collisionData.unknown, data + ptr, sizeof(Vec3D));
							ptr += sizeof(Vec3D);

							collisionInfo.push_back(collisionData);
						} catch (...) {
							return;
						}
					}
				}
			}	
			// --

			// TODO:  This isn't null bytes,  its a counter of a new chunk of data
			//ptr += 4; // skip over null bytes at the end of data chunk
			// Counter for another data chunk
			memcpy(&counter, data + ptr, 4);
			ptr += 4;

			for (unsigned int i=0; i<counter; i++) 
			{
				ptr += 456;
			}
		}
		// --

		// Num of meshes
		memcpy(&counter, data + ptr, 4);
		ptr += 4;

		// output some debug info
		wxLogMessage(_T("Render Pass Count: %i"), counter);

		if (counter > 5000) 
		{
			// Range check for invalid values
			wxLogMessage(_T("Error: had render pass count of %i.  Exiting function."), counter);
			return;
		}
		
		for (unsigned int i=0; i<counter; i++) 
		{
			// Error handler
			try {

				ptr += 9; // 9 null bytes ?

				memcpy(&renderID, data + ptr, 4);
				ptr += 4;
				wxLogMessage(_T("Render ID: %i"), renderID);

				// no point adding it to the stack, unless we actually have an id - basic error check
				if (renderID)
					renderInfo.push_back(new RenderInfo(renderID));

			} catch (...) {
				break;
			}
		}

	} catch (...) {
		wxLogMessage(_T("Error: Failed to load Object Type 4"));
		return;
	}
}



/// As above
void CObject::LoadType5(const unsigned char *data, unsigned int ptr)
{	
	wxLogMessage(_T("ptr in LoadType5 initially set to : %i"), ptr);

	unsigned int iUnk = 0;
	//unknownData1 unkData1;
	unsigned int counter = 0;
	CollisionInfo collisionData;
	unsigned int renderID = 0;
	unsigned int x = 0;
	unsigned int bytesOfZeroData[20];

	// Error handler
	try {

		while(renderID==0)
		{
			memcpy(&renderID, data + ptr, 4); // world texture id
			ptr += 4;
			bytesOfZeroData[x]=renderID;
			wxLogMessage(_T("RenderID %i"), renderID);
			x++;
		}

		wxLogMessage(_T("Encountered %i bytes of data before we reached a non-zero number!!!!"), x*4);
		wxLogMessage(_T("Render ID: %i"), renderID);
		memcpy(&renderID, data + ptr, 4); // world texture id
		ptr += 4;
		wxLogMessage(_T("Render ID: %i"), renderID);

		memcpy(&invTex, data + ptr, 4); // inventory texture id
		ptr += 4;
		wxLogMessage(_T("Inventory Texture ID: %i"), invTex);

		memcpy(&mapTex, data + ptr, 4); // Get the minimap texture id
		ptr += 4;
		wxLogMessage(_T("Minimap Texture ID: %i"), mapTex);

		memcpy(&iUnk, data + ptr, 4);
		ptr += 4;
		wxLogMessage(_T("Unknown ID: %i"), iUnk);

		memcpy(&iUnk, data + ptr, 4); 
		ptr += 4;
		wxLogMessage(_T("Unknown ID: %i"), iUnk);

		// Counter for number of records of unknown data
		memcpy(&counter, data + ptr, 4);
		ptr += 4;

		// range error check
		if ( counter > 10000) // nothing should have more than that
			throw;

		// Unknown data chunks
		// These chunks contain info like position, rotation, and scale - I think.
		/* Uncomment this to check out the data in the chunks
		for (unsigned int i=0; i<counter; i++) {
			memcpy(&unkData1, data + ptr, sizeof(unknownData1));
			ptr += sizeof(unknownData1);
		}
		*/
		ptr += sizeof(unknownData1) * counter;


		// skip over unknown data
		ptr += 108; 

		// 4 bytes that seem to contain boolean info about the object
		memcpy(&bValue1, data+ptr, 1);
		ptr++;
		memcpy(&bValue2, data+ptr, 1);
		ptr++;
		memcpy(&bValue3, data+ptr, 1);
		ptr++;
		memcpy(&bWalkData, data+ptr, 1);
		ptr++;

		// skip over more unknown data
		ptr += 7;

		// range check and if statement - some type 5 objects don't have any of this data - must be a boolean value somewhere !?
		// possible in the above 119 bytes
		if (bWalkData) {
			// Counter
			memcpy(&counter, data + ptr, 4);
			ptr += 4;

			// range error check
			if ( counter > 10000) // nothing should have more than that
				throw;

			// unknown data chunk(s)
			// These chunks contain information like colision detection, walkable areas, etc - and some other data im unsure of.
			for (unsigned int i=0; i<counter; i++) {
				// Error handler
				try {
					memcpy(&collisionData.nVectors, data + ptr, 4); 
					ptr += 4;

					memcpy(&collisionData.bounds, data + ptr, (sizeof(Vec3D) * collisionData.nVectors));
					ptr += (sizeof(Vec3D) * collisionData.nVectors);
					
					memcpy(&collisionData.upVector, data + ptr, sizeof(Vec3D));
					ptr += sizeof(Vec3D);

					memcpy(&collisionData.order, data + ptr, sizeof(uint16) * 6);
					ptr += sizeof(uint16) * 6;
					
					memcpy(&collisionData.unknown, data + ptr, sizeof(Vec3D));
					ptr += sizeof(Vec3D);

					collisionInfo.push_back(collisionData);

				} catch (...) {
					throw;
				}
			}

			// Counter for another data chunk similar to above
			memcpy(&counter, data + ptr, 4);
			ptr += 4;

			// Error check
			if (counter < 1000) { // anything not within this range is probably bad
				unsigned int tempCounter = counter;
				for (unsigned int j=0; j<tempCounter; j++) {

					// real counter to the number of chunks
					memcpy(&counter, data + ptr, 4); // Counter for the data chunk, identical to the above data chunk
					ptr += 4;

					// unknown data chunk(s)
					for (unsigned int i=0; i<counter; i++) {
						// Error handler
						try {

							// The "number of vectors" basically determines if its rendering as quad or a triangle
							memcpy(&collisionData.nVectors, data + ptr, 4); 
							ptr += 4;

							// Vertex data for above vectors
							memcpy(&collisionData.bounds, data + ptr, (sizeof(Vec3D) * collisionData.nVectors));
							ptr += (sizeof(Vec3D) * collisionData.nVectors);
							
							memcpy(&collisionData.upVector, data + ptr, sizeof(Vec3D));
							ptr += sizeof(Vec3D);

							memcpy(&collisionData.order, data + ptr, sizeof(uint16) * 6);
							ptr += sizeof(uint16) * 6;
							
							memcpy(&collisionData.unknown, data + ptr, sizeof(Vec3D));
							ptr += sizeof(Vec3D);

							collisionInfo.push_back(collisionData);

						} catch (...) {
							throw;
						}
					}
				}
			}	
			// --

			ptr += 4; // skip over null bytes at the end of data chunk
		}
		// --

		// Num of meshes
		memcpy(&counter, data + ptr, 4);
		ptr += 4;

		// output some debug info
		wxLogMessage(_T("Render Pass Count: %i"), counter);

		if (counter > 5000) { // Range check for invalid values
			wxLogMessage(_T("Error: had render pass count of %i.  Exiting function."), counter);
			return;
		}
		
		for (unsigned int i=0; i<counter; i++) {
			// Error handler
			try {

				ptr += 9; // 9 null bytes ?

				memcpy(&renderID, data + ptr, 4);
				ptr += 4;
				wxLogMessage(_T("Render ID: %i"), renderID);

				// no point adding it to the stack, unless we actually have an id - basic error check
				if (renderID)
					renderInfo.push_back(new RenderInfo(renderID));

			} catch (...) {
				break;
			}
		}

	} catch (...) {
		wxLogMessage(_T("Error: Failed to load Object Type 5"));
		return;
	}
}



void CObject::LoadType9(const unsigned char *data, unsigned int ptr)
{
	unsigned int iUnk = 0;
	wxLogMessage(_T("ptr in LoadType9 initially set to : %i"), ptr);

	// Error handler
	try {

		memcpy(&renderID, data + ptr, 4); // world texture id
		ptr += 4;
		wxLogMessage(_T("Render ID: %i"), renderID);

		memcpy(&invTex, data + ptr, 4); // inventory texture id
		ptr += 4;
		wxLogMessage(_T("Inventory Texture ID: %i"), invTex);

		memcpy(&mapTex, data + ptr, 4); // Get the minimap texture id
		ptr += 4;
		wxLogMessage(_T("Minimap Texture ID: %i"), mapTex);

		memcpy(&iUnk, data + ptr, 4);
		ptr += 4;
		wxLogMessage(_T("Unknown ID: %i"), iUnk);

		memcpy(&iUnk, data + ptr, 4); 
		ptr += 4;
		wxLogMessage(_T("Unknown ID: %i"), iUnk);

		if (!renderID)
			return; //if there was no ID given, then nfi what to do - so exit.

		
		// Get the render info
		renderInfo.push_back(new RenderInfo(renderID));

	} catch (...) {
		wxLogMessage(_T("Error loading type 9 object."));
	}
}



void CObject::LoadType13(const unsigned char *data, unsigned int ptr)
{
	unsigned int counter = 0;
	unsigned char letter = 0;
	unsigned int skeletonID = 0;
	unsigned int renderID = 0;
	wxLogMessage(_T("ptr in LoadType13 initially set to : %i"), ptr);

	Vec3D p; // position ? Rotation ? scale ?

	wxString text1;
	wxString text2;
	wxString text3;

	try {
		ptr += 158;

		memcpy(&counter, data+ptr, 4);
		ptr += 4;

		// Text
		text1 = "";
		for(unsigned int i=0; i<counter; i++) 
		{
			memcpy(&letter, data+ptr+(i * 2), 1);
			text1.Append(letter);
			//wxLogMessage(text1);
		}
		
		ptr += (counter * 2);
		if (!text1.IsEmpty())
			wxLogMessage(text1);
		// --

		ptr += 91;
		memcpy(&counter, data+ptr, 4);
		ptr += 4;

		// skip over the unknown data
		ptr += (3 * 4 * counter); // 3 values, of 4 bytes, 

		memcpy(&counter, data+ptr, 4);
		ptr += 4;
		if (counter > 0)
		{
			ptr += 40;
		}	

		ptr += 121;

		memcpy(&counter, data+ptr, 4);
		ptr += 4;

		// Text
		text2 = "";
		for(unsigned int i=0; i<counter; i++) {
			memcpy(&letter, data+ptr+(i * 2), 1);
			text2.Append(letter);
			//wxLogMessage(text2);
		}
		ptr += (counter * 2);
		if (!text2.IsEmpty()) {
			text2.Replace("\\n", "\n", true);
			//wxLogMessage(text2);
		}
		// --

		memcpy(&counter, data+ptr, 4);
		ptr += 4;

		// Text
		text3 = "";
		for(unsigned int i=0; i<counter; i++) {
			memcpy(&letter, data+ptr+(i * 2), 1);
			text3.Append(letter);
			wxLogMessage(text3);

		}
		ptr += (counter * 2);
		if (!text3.IsEmpty()) {
			text3.Replace("\\n", "\n", true);
			wxLogMessage(text3);
		}
		// --


		ptr += 229;	// skip unknown data

		// Skeleton scale
		memcpy(&scale, data+ptr, sizeof(Vec3D));
		ptr += sizeof(Vec3D);

		// Get the skeleton id
		memcpy(&skeletonID, data+ptr, 4);
		ptr += 4;
		if(skeletonID == 0)
		{
			throw;
		}
		
		// Create and load the skeleton
		skeleton = new Skeleton(skeletonID);

		// set the scale
		skeleton->SetScale(scale);

		// Log some debug info
		wxLogMessage(_T("Skeleton ID: %i\nScake: x=%d y=%d z=%d"), 
			skeletonID, scale.x, scale.y, scale.z);
		
		ptr += 18; // null

		memcpy(&counter, data+ptr, 4);
		ptr += 4;

		for (unsigned int i=0; i<counter; i++) {
			ptr += 4;
			memcpy(&renderID, data+ptr, 4);
			ptr += 4;

			memcpy(&p, data+ptr, sizeof(Vec3D));
			ptr += sizeof(Vec3D);

			wxLogMessage(_T("Render ID: %i"), renderID); 

			renderInfo.push_back(new RenderInfo(renderID));
		}


		// Setup the skeleton stuff
		if (skeleton) {
			for (size_t i=0; i<renderInfo.size(); i++) {
				if (renderInfo[i]->mesh && !renderInfo[i]->joint.IsEmpty())
					skeleton->SetMesh(renderInfo[i]->joint, renderInfo[i]->mesh);
			}
		}

	} catch (...) {
		//
	}
}


void CObject::LoadType15(const unsigned char *data, unsigned int ptr)
{
	unsigned int iUnk = 0;

	// Error handler
	try {

		memcpy(&renderID, data + ptr, 4); // world texture id
		ptr += 4;
		wxLogMessage(_T("Render ID: %i"), renderID);

		memcpy(&invTex, data + ptr, 4); // inventory texture id
		ptr += 4;
		wxLogMessage(_T("Inventory Texture ID: %i"), invTex);

		memcpy(&mapTex, data + ptr, 4); // Get the minimap texture id
		ptr += 4;
		wxLogMessage(_T("Minimap Texture ID: %i"), mapTex);

		memcpy(&iUnk, data + ptr, 4);
		ptr += 4;
		wxLogMessage(_T("Unknown ID: %i"), iUnk);

		memcpy(&iUnk, data + ptr, 4); 
		ptr += 4;
		wxLogMessage(_T("Unknown ID: %i"), iUnk);

		/*
		if (!renderID)
			return; //if there was no ID given, then nfi what to do - so exit.

		
		// Get the render info
		renderInfo.push_back(new RenderInfo(renderID));
		*/

	} catch (...) {
		wxLogMessage(_T("Error loading type 9 object."));
	}
}




void CObject::DrawWalkable()
{
	// Not sure if this is better than my older method of doing it,  but it seems to be the
	// way that Wolfpack "designed" the data to be used.

	GLenum mode = 0;

	glTranslatef(0.0f, 0.4f, 0.0f);
	glColor3f(1.0f,0.2f,0.2f); // set to red, so its visuall obvious

	for (size_t i=0; i<collisionInfo.size(); i++) {
		if (collisionInfo[i].nVectors == 3)
			mode = GL_TRIANGLES;
		else if (collisionInfo[i].nVectors == 4)
			mode = GL_QUADS;
		else
			break;

		glBegin(mode);
			// Draw the vectors
			for (unsigned int x=0; x<collisionInfo[i].nVectors; x++) {
				glVertex3fv(collisionInfo[i].bounds[x]);
			}
		glEnd();
	}

	glColor3f(1.0f,1.0f,1.0f); // set back to white, default
	glTranslatef(0.0f, -0.4f, 0.0f);
}


void CObject::DrawBounds()
{	
	for (size_t i=0; i<renderInfo.size(); i++) {
		glPushMatrix();
		renderInfo[i]->DrawBounds();
		glPopMatrix();
	}	
}


void CObject::DrawSkeleton()
{
	if (skeleton) {
		glDisable(GL_DEPTH_TEST);
		glDisable(GL_TEXTURE_2D);
		skeleton->DrawSkeleton();
		glEnable(GL_DEPTH_TEST);
		glEnable(GL_TEXTURE_2D);
	}
}


void CObject::DrawModel()
{
	if (skeleton) {
		skeleton->DrawModel();
	} else {
		for (size_t i=0; i<renderInfo.size(); i++)
			renderInfo[i]->Draw();
	}
}


void CObject::Animate()
{
	// if not skeleton or no animation frames on the skeleton, exit func
	if (skeleton)
		skeleton->Animate();	
}


CObject::~CObject()
{
	wxDELETE(icon);
	wxDELETE(skeleton);

	renderInfo.clear();
	collisionInfo.clear();
}
