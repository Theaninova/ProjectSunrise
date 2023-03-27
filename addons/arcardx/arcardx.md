# ArcarDX
ArcarDX /ɑːˈkeɪd/

## Surface



## Engine

Inputs are

* Rev limit $$R_{max}$$
* Torque $$\tau$$ in $$N\cdot m$$
* Efficiency $$\eta$$ as a function of revs
* Mass of the car $$m_{car}$$
* Mass of the rotating parts inside the engine not connected by a clutch $$m_{\tau}$$
* Kinetic friction coefficient of lubricated steel on steel $$\mu_{steel}=0.029$$
* Rolling resistance coefficient $$C_{rr}=0.015$$
* Traction coefficient $$C_t$$ where a usual value is $$\mu_s=0.4$$
* Drift coefficient $$C_d$$ where the sliding friction between concrete and rubber is $$\mu_k=0.85$$
* Gravity $$g=9.81\frac{m}{s^2}$$
* Downforce $$F_{\downarrow}$$ as a function of speed
* Tire Radius $$r_{cm}$$ with $$r = \frac{r_{cm}}{100}$$
* User Inputs
	* Throttle $$I_t$$
	* Brake $$I_b$$
	* Steering $$I_s$$
* Passed time $$t_{\Delta}$$
* Drift angle $$\theta_{\Delta}$$
* Steering angle $$\sigma$$

Clamp function $$_l\lfloor x\rceil^u$$

Normal force $$N_{\downarrow}=m_{car}\cdot(g + F_{\downarrow})$$

Rolling resistance force $$F_{rr}=N_{\downarrow} \cdot C_{rr}$$

Usable traction $$F_t=N_{\downarrow}\cdot C_{td}$$ where $$C_{td}=C_t$$ if the static friction has not been overcome yet and $$C_{td}=C_d$$ if static friction has been overcome. meaning the car is in a slide

Brake force $$F_b=F_{rr} \cdot sin(\theta_{\Delta}) + F_t\cdot \stackrel{0}{\lfloor} cos(\theta_{\Delta})+I_b\stackrel{1}{\rceil}$$ that transitions between rolling resistance and traction as the drift angle approaches $$90^{\circ}$$.

Lateral force $$F_l=asin(\sigma+\theta_{\Delta})\cdot v$$

The force acting on the car $$\overrightarrow{F}=$$

Torque $$\tau$$ is defined as the ability to produce a change in rotational momentum of a body, where the SI-unit $$N\cdot m$$ is the ability to accelerate $$1kg$$ at a radius of $$1m$$ with $$1\frac{m}{s^2}$$

The effective torque $$\tau_{\eta}$$ is $$\tau\cdot\eta\cdot I_t$$



The velocity $$v_{RPM}$$ for the engine's rotation per minute $$R$$ is
$$v_{RPM}=2\pi\frac{R}{60}$$
The angular momentum $$L$$
$$L=m*v*r$$

The actual power output in $$kW$$
$$P=power(R)*I_t$$

The torque $$\tau$$ in $$1J = 1Nm$$ where $$\theta = 90°$$ and $$sin(\theta)=1$$ of the engine is therefore
$$\tau = P*L$$ 
