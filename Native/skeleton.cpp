
#include "skeleton.h"
#include "cachelib.h"

Bone::Bone()
{
	id = 0;
	mesh = 0;
	nChildren = 0;
	parent = 0;
	children = 0;

	dir = Vec3D(0,0,0);
	axis = Vec3D(0,0,0);;
	length = 0.0f;

	pos = Vec3D(0,0,0);
	rot = Quaternion(0,0,0,0);
	scale = Vec3D(1,1,1);

	mat.unit();
	mrot.unit();

	setup = false;
	flip = false;
}

Bone::~Bone()
{
	animBlocks.clear();
}

unsigned int Bone::Init(const unsigned char *data)
{
	uint32 ptr = 0;
	uint32 size = 0;
	unsigned char letter = 0;
	int32 unk = 0;
	float unk2 = 0.0f;
	int16 unk3 = 0;

	try {
		// Bone id
		memcpy(&id,data,4);
		ptr += 4;

		// Joint name
		memcpy(&size, data+ptr, 4);
		ptr += 4;
		for (unsigned int i=0; i<size; i++) {
			memcpy(&letter, data+ptr+(i*2), 1);
			name.Append(letter);
		}

		ptr += (size * 2); // * 2 because its a unicode string UTF16.
		// --

		// Direction
		memcpy(&dir, data+ptr, sizeof(Vec3D));
		ptr += sizeof(Vec3D);
		// --

		// Length
		memcpy(&length, data+ptr, sizeof(float));
		ptr += sizeof(float);

		// Axis
		memcpy(&axis, data+ptr, sizeof(Vec3D));
		ptr += sizeof(Vec3D);

		// convert from radians to degrees,  opengl uses degrees.
		axis *= RAD2DEG;
		// --

		// translation and rotation boolean values in the form of a string "tx ty tz rx ry rz", and "rx ry rz"
		memcpy(&size, data+ptr, 4);
		ptr += 4;
		//memcpy(tempBuf, data+ptr, (size * 2)); // * 2 because its a unicode string UTF16.
		// Who cares about the above info,  seems to be irrelevant for the time being.
		ptr += (size * 2);
		// --

		// Skip over the last of the data - always null, Shadowbane doesn't make full use of the ASF/AMC file format
		ptr += 36;

		memcpy(&flip, data+ptr, 1);		// Is the mesh "mirrored" from the other side ?
		ptr++;

		//memcpy(&bUnknown, data+ptr, 1); // 0x01 - unknown flag ?
		ptr++;

		memcpy(&nChildren, data+ptr, 4); // number of child nodes
		ptr += 4;

		wxLogMessage(_T("Bone ID: %i, Num Children: %i, Joint:%s, Flip?:%i, Unknown Flag:%i"), id, nChildren, name.c_str(), flip, unk);
	} catch (...) {
		// error - probably error due totrying to copy invalid memory
	}

	return ptr;
}

void Bone::Setup(std::vector<Bone> *allbones, Bone *p)
{
	parent = p;		// set parent flag
	setup = true;	// this bone has been processed, set flag to true

	/*
	if (parent) {
		Vec3D p = dir * length;
		//p.y = p.y * 0.85f;
		pivot = parent->pivot + p;
	} else {
		pivot = Vec3D(0,0,0);
	}
	*/

	// Clear any previous memory first
	if (nChildren > 0) {
		wxDELETEA(children);
		children = new Bone*[nChildren];
	} else {
		children = NULL;
		return;
	}

	// Children nodes
	for(uint32 i=0; i<nChildren; i++) {
		/*
		for(std::vector<Bone>::iterator j = allbones->begin(); j != allbones->end(); ++j) {
			if ((*j).setup == false) {
				//children.push_back(&allbones[j]);
				(*j).Setup(allbones, this);
				children[i] = *(*j);
				break;
			}
		}
		*/

		for (size_t j=0; j<allbones->size(); j++) {
			if (allbones->at(j).setup == false) {
				allbones->at(j).Setup(allbones, this);
				children[i] = &allbones->at(j);
				break;
			}
		}
	}
	
}

#include "eulerangle.h"

void Bone::CalcMatrix(const unsigned int frame)
{
	// All the animation stuff is in radians,  convert to degrees.
	if (frame < animBlocks.size()) {
		pos = animBlocks[frame]->pos;
		rot = animBlocks[frame]->rot;
		scale = animBlocks[frame]->scale;

		const Quaternion q(rot);

		//r = Matrix::newQuatRotate(q) * 1.0f;
		//r = Matrix::eulerAngle(q) * 1.0f;
		//r = Matrix::eulerAngle2(q) * RAD2DEG;
		//rot = Eul_FromQuat(q, EulOrdZYXs) * RAD2DEG;
		
		// local transformation matrix
		//Matrix m;
		
		/*
		m.unit();
		//m.translation(axis);
		
		// Position
		if (parent)
			m *= Matrix::newTranslation(parent->dir * parent->length);
		else
			m *= Matrix::newTranslation(pos);

		*/
		// Rotation
		//m *= Matrix::newQuatRotate(q);

		/*
		// Scale
		//m *= Matrix::newScale(scale);
		// Invert
		//m *= Matrix::newTranslation(q*-1.0f);


		// This is relative to its parent
		//if (parent)
		//	mat = parent->mat * m;
		//else 
			mat = m;

		*/

		//if (parent)
		//	mrot = parent->mrot * Matrix::newQuatRotate(q);
		//else 
			mrot = Matrix::newQuatRotate(q);
			//mrot *= Matrix::newTranslation(dir * length);
			//mrot.translation(dir * length);
		

	} else {
		if (parent) {
			mat = parent->mat;
			mrot = parent->mat;
		} else { // if no animation, for root set to the default
			mat.unit();
			mrot.unit();
		}
	}
	

	for (unsigned int i=0; i<nChildren; i++) {
		children[i]->CalcMatrix(frame);
	}
}

inline void Bone::DrawMesh()
{
	//Store the current ModelviewMatrix (before adding the translation part)
	glPushMatrix();

	//Tranform (rotate) from the local coordinate system of this bone to it's parent
	//glMultMatrixf(mat);  

	// translate matrix for the root bone only
	if (!parent)
		glTranslatef(pos.x, pos.y, pos.z);

	glMultMatrixf(mrot);
	

	glScalef(scale.x, scale.y, scale.z);
	
	if (mesh) {
		// draw mesh attached to bone
		mesh->DrawModel();

		//Compute tx, ty, tz - translation from pBone to it's child (in local coordinate system of pBone)
		Vec3D p = dir * length;

		//translate to the center of the bone
		//glTranslatef(p.x/2.0, p.y/2.0, p.z/2.0);
		glTranslatef(p.x, p.y, p.z);

		/*
		glPushMatrix();
		//Compute the angle between the canonical pose and the correct orientation
		Vec3D z_dir(0,0,1);
		Vec3D r_axis = axis.cross(z_dir);
		float theta = atan2(r_axis.length(), axis.dot(z_dir));
		
		glRotatef(theta * RAD2DEG, r_axis.x, r_axis.y, r_axis.z);
		// ----
		*/

		
	}


	for (size_t i=0; i<nChildren; i++)
		children[i]->DrawMesh();

	glPopMatrix();
}

inline void Bone::DrawBone()
{
	GLUquadricObj *qObj = gluNewQuadric();
	gluQuadricDrawStyle(qObj, (GLenum) GLU_FILL);
	gluQuadricNormals(qObj, (GLenum) GLU_SMOOTH);

	//Store the current ModelviewMatrix (before adding the translation part)
	glPushMatrix();

	//Tranform (rotate) from the local coordinate system of this bone to it's parent
	//glMultMatrixf(mat);
	//glLoadMatrixf(mat.m);
	
	// translate matrix for the root bone only
	if (!parent)
		glTranslatef(pos.x, pos.y, pos.z);

	glMultMatrixf(mrot);

	glScalef(scale.x, scale.y, scale.z);
	
	if (length > 0) {
		//Compute tx, ty, tz - translation from pBone to it's child (in local coordinate system of pBone)
		Vec3D p = dir * length;

		//translate to the center of the bone
		//glTranslatef(p.x/2.0, p.y/2.0, p.z/2.0);
		glTranslatef(p.x, p.y, p.z);		

		// Draw joint
		glColor3f(0.1f,0.9f,0.3f);
		gluSphere(qObj, 0.1f, 8, 8);

		// draw "bone"
		glColor3f(0.5f, 0.2f, 0.2f);
		glBegin(GL_LINES);
		glVertex3f(0,0,0);
		glVertex3fv(-p);
		glEnd();

		//glPopMatrix();
	}


	for (size_t i=0; i<nChildren; i++)
		children[i]->DrawBone();

	glPopMatrix();
}








Skeleton::Skeleton(const unsigned int fileID)
{
	nBones = 0;
	nFrames = 0;
	curFrame = 0;
	nAnimBones = 0;

	const unsigned char *data = skeletonArchive.getFileByID(fileID);

	// Error check
	if (!data)
		return;

	const unsigned int dataSize = skeletonArchive.getFileSizeByID(fileID);

	Create(data, dataSize);
}

Skeleton::Skeleton(const unsigned char *data, const unsigned int dataSize)
{
	nBones = 0;
	nFrames = 0;
	curFrame = 0;
	nAnimBones = 0;

	Create(data, dataSize);
}

void Skeleton::Create(const unsigned char *data, const unsigned int dataSize)
{
	uint32 ptr = 0;
	uint32 size = 0;
	uint32 id = 0;
	unsigned char letter = 0;

	// size and string
	memcpy(&size, data, 4);
	ptr += 4;
	for (unsigned int i=0; i<size; i++) {
		memcpy(&letter, data+ptr+(i*2), 1);
		name.Append(letter);
	}
	ptr += (size * 2); // * 2 because its a unicode string 
	// Log some info for debugging
	wxLogMessage(_T("Skeleton Name: %s"), name.c_str());
	// --

	// number of animations available to this skeleton
	memcpy(&size, data+ptr, 4);
	ptr += 4;

	// This data contains reference ID's to motion.cache
	for (unsigned int i=0; i<size; i++) {
		ptr += 4;
		memcpy(&id, data+ptr, 4);
		ptr += 4;
		
		if (id > 0)
			animations.push_back(id);

		ptr += 8;
	}
	
	// Start reading in bones
	// the +90 is just some extra precaution to make sure there is enough data in the buffer to create a bone
	while ((ptr + 90) < dataSize) {
		Bone b;
		ptr += b.Init(data+ptr);

		bones.push_back(b);

		nBones++;
	}
	

	// Now setup the bones.
	if (bones.size() > 0)
		bones[0].Setup(&bones, NULL);


	if (animations.size() > 0)
		SetAnimation(animations[0]);
}

Bone* Skeleton::GetBone(const wxString bone)
{
	for (size_t i=0; i<bones.size(); i++) {
		if (bones[i].name == bone)
			return &bones[i];
	}

	return 0;
}

void Skeleton::DrawModel()
{
	glColor3f(1.0f, 1.0f, 0.1f);
	bones[0].DrawMesh();
}

void Skeleton::DrawSkeleton()
{
	//glDisable(GL_DEPTH_TEST);
	glColor3f(1.0f, 1.0f, 0.1f);
	bones[0].DrawBone();
	//glEnable(GL_DEPTH_TEST);
}

void Skeleton::Animate()
{
	// if no animation frames, exit func
	if (!nFrames)
		return;
	
	bones[0].CalcMatrix(curFrame);

	curFrame++;
	if (curFrame >= nFrames)
		curFrame = 0;
}

void Skeleton::SetAnimation(unsigned int fileID)
{
	const unsigned char *data = motionArchive.getFileByID(fileID);
	const unsigned int dataSize = motionArchive.getFileSizeByID(fileID);

	// Error check
	if (!data)
		return;

	unsigned int ptr = 0;
	unsigned int counter = 0;
	wxChar letter = 0;


	// Clear the data from the last animation
	for (size_t i=0; i<bones.size(); i++) {
		bones[i].animBlocks.clear();
	}

	nFrames = 0;
	nAnimBones = 0;

	// Length of string
	memcpy(&counter, data, 4);
	ptr += 4;

	// Skip over string
	ptr += (counter * 2);

	// Number of frames in the animation
	memcpy(&nFrames, data+ptr, 4);
	ptr += 4;

	// skip over unknown data
	ptr += 27;

	// number of bones affected by the animation
	memcpy(&nAnimBones, data+ptr, 4);
	ptr += 4;

	// Array of pointers to key bones
	Bone **bonePtr = new Bone*[nAnimBones];
	
	// Find the pointer to the bone that will be affected by the animation
	for (unsigned int i=0; i<nAnimBones; i++) {
		wxString boneName = "";

		// length of string (name of bone)
		memcpy(&counter, data+ptr, 4);
		ptr += 4;
		
		// read in string
		for (unsigned int j=0; j<counter; j++) {
			memcpy(&letter, data+ptr, sizeof(wxChar));
			ptr += 2;
			boneName.Append(letter);
		}

		bonePtr[i] = GetBone(boneName);
	}

	// Total number of animation blocks
	memcpy(&counter, data+ptr, 4);
	ptr += 4;

	// Read in all the animation blocks
	for (unsigned int x=0; x<nFrames; x++) {
		for (unsigned int y=0; y<nAnimBones; y++) {

			AnimBlock *b = new AnimBlock;
			memcpy(b, data+ptr, sizeof(AnimBlock));
			ptr += sizeof(AnimBlock);

			// convert from radians to degrees
			//b->rot *= RAD2DEG;
			const Vec4D old = b->rot;
			b->rot.x = old.y;		
			b->rot.y = old.w;	
			b->rot.z = old.z;	
			b->rot.w = old.x;
			
			if (y==0) {
				b->rot.y = old.z;
				b->rot.z = old.w;
			}

			if(bonePtr[y])
				bonePtr[y]->animBlocks.push_back(b);
		}
	}

	wxDELETEA(bonePtr);
}


void Skeleton::SetMesh(wxString boneName, Model *mesh)
{
	Bone *b = GetBone(boneName);
	if(b) {
		b->mesh = mesh;

		if (boneName[0] == 'R') {
			boneName[0] = 'L';
			b = GetBone(boneName);

			if(b && b->flip)
				b->mesh = mesh;
		}
	}
}

Skeleton::~Skeleton()
{
	animations.clear();
	bones.clear();
}


