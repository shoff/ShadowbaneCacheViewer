
#include "stdafx.h"
#include "canvas.h"

const float piover180 = 0.0174532925f;
const float rad2deg = 57.295779513f;

IMPLEMENT_CLASS(Canvas, wxGLCanvas)

BEGIN_EVENT_TABLE(Canvas, wxGLCanvas)
    EVT_SIZE(Canvas::OnSize)
    EVT_PAINT(Canvas::OnPaint)
    EVT_MOUSE_EVENTS(Canvas::OnMouse)
	//EVT_KEY_DOWN(Canvas::OnKey)

    EVT_TIMER(ID_CANVAS_TIMER, Canvas::OnTimer)
END_EVENT_TABLE()

Canvas::Canvas(wxWindow *parent,
			   wxWindowID id, 
			   const wxPoint& pos, 
			   const wxSize& size, 
			   long style, 
			   const wxString& name, 
			   int *attribList,
			   const wxPalette& palette,
			   const int pixelFormat)
    : wxGLCanvas(parent, id, pos, size, style|wxFULL_REPAINT_ON_RESIZE, name, attribList, palette)/*, pixelFormat)*/
{
	wxLogMessage(_T("Creating OpenGL Canvas."));

	tex = NULL;
	model = NULL;
	skeleton = NULL;
	obj = NULL;
	ground = NULL;
	zone = NULL;

	drawBounds = false;
	drawWalkable = false;
	drawSkeleton = false;
	drawGround = false;

	renderMode = GL_FILL;
	
	vPos = Vec3D(0,0,-20.0f);
	vPos0 = Vec3D(0,0,0);
	vRot = vRot0 = Vec3D(0,0,0);

    timer.SetOwner(this, ID_CANVAS_TIMER);
	timer.Start(50); // 20 frames per second

	InitGL();
	InitView();

	Show(true);
}

Canvas::~Canvas()
{
	wxDELETE(tex);
	wxDELETE(model);
	wxDELETE(skeleton);
	wxDELETE(obj);
	wxDELETE(ground);
	wxDELETE(zone);
}


void Canvas::OnSize(wxSizeEvent& event)
{
	SetCurrent();

	wxGLCanvas::OnSize(event);

	InitGL();
	InitView();
}

void Canvas::OnTimer(wxTimerEvent& event)
{
	Refresh(false);
}

void Canvas::OnPaint(wxPaintEvent& WXUNUSED(event))
{
	// Set this window handler as the reference to draw to.
	wxPaintDC dc(this);

	Render();
}

void Canvas::OnMouse(wxMouseEvent& event)
{
	int px = event.GetX();
	int py = event.GetY();
	int pz = event.GetWheelRotation();

	// mul = multiplier in which to multiply everything to achieve a sense of control over the amount to move stuff by
	float mul = 1.0f;
	if (event.m_shiftDown)
		mul /= 10;
	if (event.m_controlDown)
		mul *= 10;
	if (event.m_altDown)
		mul *= 50;


	if (event.ButtonDown()) {
		mx = px;
		my = py;
		vRot0 = vRot;
		vPos0 = vPos;

	} else if (event.Dragging()) {
		int dx = mx - px;
		int dy = my - py;

		if (event.LeftIsDown()) {
			vRot.x = vRot0.x - (dy / 2.0f); // * mul);
			vRot.y = vRot0.y - (dx / 2.0f); // * mul);

		} else if (event.RightIsDown()) {
			mul /= 100.0f;

			vPos.x = vPos0.x - dx*mul;
			vPos.y = vPos0.y + dy*mul;

		} else if (event.MiddleIsDown()) {
			if (!event.m_altDown) {
				mul = (mul / 20.0f) * dy;

				Zoom(mul, false);
				my = py;

			} else {
				mul = (mul / 1200.0f) * dy;

				Zoom(mul, true);
				my = py;
			}
		}

	} else if (event.GetEventType() == wxEVT_MOUSEWHEEL) {
		if (pz != 0) {
			mul = (mul / 120.0f) * pz;
			if (!wxGetKeyState(WXK_ALT)) {
				Zoom(mul, false);
			} else {
				mul /= 50.0f;
				Zoom(mul, true);
			}
		}
	}

	if (event.GetEventType() == wxEVT_ENTER_WINDOW)
		SetFocus();
}

void Canvas::Zoom(float f, bool rel)
{
	if (rel) {
		float cosx = cos(vRot.x * piover180);
		vPos.x += cos(vRot.y * piover180) * cosx * f;
		vPos.y += sin(vRot.x * piover180) * sin(vRot.y * piover180) * f;
		vPos.z += sin(vRot.y * piover180) * cosx * f;
	} else {
		vPos.z -= f;
	}
}

void Canvas::InitGL()
{
	SetCurrent();

	glEnable(GL_TEXTURE_2D);
	glEnable(GL_COLOR);

	glShadeModel(GL_SMOOTH);
	glHint(GL_PERSPECTIVE_CORRECTION_HINT, GL_NICEST);

	glEnable(GL_DEPTH_TEST);
	glDepthFunc(GL_LEQUAL);	
}

void Canvas::InitView()
{
	int w=0, h=0;
	GetClientSize(&w, &h);

	SetCurrent();
	glViewport(0, 0, (GLint) w, (GLint) h);

	float aspect = float(w / h);

	glMatrixMode(GL_PROJECTION);
    glLoadIdentity();	

	// 50 degrees, field of view, standard aspect ratio.
	gluPerspective(60.0f, aspect, 0.1f, 256.0f);
	
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
}

void Canvas::InitLighting()
{
	glEnable(GL_LIGHTING);
	glEnable(GL_LIGHT0);
	glEnable(GL_NORMALIZE);

	Vec4D ambience = Vec4D(0.3f, 0.3f, 0.3f, 1.0f);
	Vec4D diffuse = Vec4D(8.0f, 8.0f, 8.0f, 8.0f);
	Vec4D specular = Vec4D(0.0f, 0.0f, 0.0f, 1.0f);
	Vec4D pos = Vec4D(0.0f, 0.15f, 1.0f, 1.0f);	// light from behind

	glLightfv(GL_LIGHT0, GL_DIFFUSE, diffuse);
	glLightfv(GL_LIGHT0, GL_AMBIENT, ambience);
	glLightfv(GL_LIGHT0, GL_SPECULAR, specular);
	glLightfv(GL_LIGHT0, GL_POSITION, pos);

	glLightf(GL_LIGHT0, GL_CONSTANT_ATTENUATION, 0.1f);
	glLightf(GL_LIGHT0, GL_LINEAR_ATTENUATION, 0.0f);
	glLightf(GL_LIGHT0, GL_QUADRATIC_ATTENUATION, 0.0f);
	glLightf(GL_LIGHT0, GL_SPOT_CUTOFF, 180.0f);	// Makes the lighting directional
}


void Canvas::LoadObject(unsigned int id)
{
	// Clear memory from previous;
	wxDELETE(obj);
	wxDELETE(model);
	wxDELETE(tex);

	unsigned char *bufPtr = objArchive.getFileByID(id);
	unsigned int fileSize = objArchive.getFileSizeByID(id);

	obj = new CObject(bufPtr);
	
	vPos = Vec3D(0, -5.0f, -20.0f);
	vPos0 = Vec3D(0,0,0);
	vRot = vRot0 = Vec3D(0,0,0);
}


void Canvas::LoadGround(unsigned int id)
{
	wxDELETE(ground);

	if (id)
		ground = new CObject(id);

}

//Draw checker board ground plane
void Canvas::DrawGround() 
{
   int count = 0;

   const GLfloat white[] = {0.4f, 0.4f, 0.4f, 1.0f};
   const GLfloat green[] = {0.0f, 0.5f, 0.0f, 1.0f};
   const GLfloat black[] = {0.0f, 0.0f, 0.0f, 1.0f};

   glDisable(GL_TEXTURE_2D);
	
   glBegin(GL_QUADS);

	for(float i=-20.0f; i<=20.0f; i+=1.0f) {
		for(float j=-20.0f; j<=20.0f; j+=1.0f) {
			if((count%2) == 0) {
				glMaterialfv(GL_FRONT_AND_BACK, GL_AMBIENT, black);
				glMaterialfv(GL_FRONT_AND_BACK, GL_DIFFUSE, white);
				glColor3f(1.0f, 1.0f, 1.0f);
			} else {
				glMaterialfv(GL_FRONT_AND_BACK, GL_AMBIENT, black);	
				glMaterialfv(GL_FRONT_AND_BACK, GL_DIFFUSE, black);
				glColor3f(0.2f, 0.2f, 0.2f);
			}

			glNormal3f(0, 1, 0);

			glVertex3f(j,  -4, i);
			glVertex3f(j,  -4, i+1);
			glVertex3f(j+1,-4, i+1);
			glVertex3f(j+1,-4, i);
			count++;
		}
	}

   glEnd();

   glEnable(GL_TEXTURE_2D);
}

void Canvas::Render()
{
	glClearColor(0.5f, 0.5f, 0.5f, 0.0f);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

	glLoadIdentity();


	// Interface stuff ==========================
	glPushMatrix();

	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();

	glMatrixMode(GL_MODELVIEW);					// Select The Modelview Matrix
	glLoadIdentity();
	
	//glPushMatrix();

	glOrtho(0, 512, 0, 512, -1.0, 1.0);

	glDisable(GL_DEPTH_TEST);
	
	glColor3f(1.0f, 1.0f, 1.0f);

	// Renders a small inventory icon bottom left cornor
	if (obj && obj->icon) {
		obj->icon->Bind();

		glBegin(GL_QUADS);
			glTexCoord2f(0.0f, 0.0f); glVertex2i(0, 0);
			glTexCoord2f(1.0f, 0.0f); glVertex2i(64.0f, 0);
			glTexCoord2f(1.0f, 1.0f); glVertex2i(64.0f, 64.0f);
			glTexCoord2f(0.0f, 1.0f); glVertex2i(0,  64.0f);
		glEnd();
	}
	
	// Render a square displaying the texture
	if (tex) {
		tex->Bind();

		glBegin(GL_QUADS);
			glTexCoord2f(0.0f, 0.0f); glVertex2f(0.0f, 0.0f);
			glTexCoord2f(1.0f, 0.0f); glVertex2f((float)tex->width, 0.0f);
			glTexCoord2f(1.0f, 1.0f); glVertex2f((float)tex->width, (float)tex->height);
			glTexCoord2f(0.0f, 1.0f); glVertex2f(0.0f, (float)tex->height);
		glEnd();
	}

	glPopMatrix();								// Restore The Old Modelview Matrix	

	InitView();

	glEnable(GL_DEPTH_TEST);
	
	// ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

	

	// translation/scale/rtoation
	glScalef(0.5f, 0.5f, 0.5f);
	glTranslatef(vPos.x, vPos.y, vPos.z);
	glRotatef(vRot.x, 1.0f, 0.0f, 0.0f);
	glRotatef(vRot.y, 0.0f, 1.0f, 0.0f);
	glRotatef(vRot.z, 0.0f, 0.0f, 1.0f);


	glPolygonMode(GL_FRONT_AND_BACK, renderMode);

	if (drawGround) {
		if (ground) 
			ground->DrawModel();
		else
			DrawGround();
	}

	// Render a object
	if (obj) {
		obj->Animate();	// animate the object - if there is any

		obj->DrawModel();	// draw the meshes to form the model of an object

		if (drawWalkable)
			obj->DrawWalkable();

		// draw mesh dimension boundry
		if (drawBounds)
			obj->DrawBounds();	// draw bounds of an object

		if (drawSkeleton)
			obj->DrawSkeleton();
	}

	// render a skeleton
	if (skeleton) {
		skeleton->Animate();
		skeleton->DrawSkeleton();
	}

	// Render a mesh
	if (model) {
		model->DrawModel();	

		// draw mesh boundry
		if (drawBounds)
			model->DrawBounds();
	}
	
	SwapBuffers();
}

