#version 330 core
// Now the diffuse and the specular values are controlled by textures, this is what we in graphics call mapping something.
// This means they are now based on textures instead of a color, and can be controlled much better per fragment.
// This also allows us the ability to texture our objects again.

// Note: We dont have a value for the ambient, as that is mostly the same the diffuse in pretty much every single situation.
struct Material {
    sampler2D diffuse;
    sampler2D specular;
    float     shininess;
};
struct Light {
    vec3 position;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
#define NUM_LIGHTS 2    // Number of lights

uniform Light lights[NUM_LIGHTS];
//uniform Light light;
uniform Material material;
uniform vec3 viewPos;

out vec4 FragColor;

in vec3 Normal;
in vec3 FragPos;

// Now we need the texture coordinates, however we only need one set even though we have 2 textures,
// as every fragment should have the same texture position no matter what texture we are using.
in vec2 texCoord;

void main()
{
    // Each of the 3 different components now use a texture for the material values instead of the object wide color they had before.
    // Note: The ambient and the diffuse share the same texture.
    // ambient
    vec3 result = vec3(0.0);

    for (int i = 0; i< NUM_LIGHTS; i++) {
        vec3 ambient = lights[i].ambient * vec3(texture(material.diffuse, texCoord));

        // Diffuse 
        vec3 norm = normalize(Normal);
        vec3 lightDir = normalize(lights[i].position - FragPos);
        float diff = max(dot(norm, lightDir), 0.0);
        vec3 diffuse = lights[i].diffuse * diff * vec3(texture(material.diffuse, texCoord));

        // Specular
        vec3 viewDir = normalize(viewPos - FragPos);
        vec3 reflectDir = reflect(-lightDir, norm);
        float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
        vec3 specular = lights[i].specular * spec * vec3(texture(material.specular, texCoord));

        result += ambient + diffuse + specular;
    }
    FragColor = vec4(result, 1.0);
}