﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.PixelHeroes.Scripts.CharacterScrips;
using Assets.PixelHeroes.Scripts.CollectionScripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.PixelHeroes.Scripts.EditorScripts
{
    public class CharacterEditor : MonoBehaviour
    {
        public SpriteCollection SpriteCollection;
        public List<LayerEditor> Layers;
        public CharacterBuilder CharacterBuilder;
        public Sprite EmptyIcon;

        public void Start()
        {
            foreach (var layer in Layers)
            {
                if (layer.Controls)
                {
                    layer.Content = SpriteCollection.Layers.Single(i => i.Name == layer.Name);
                    layer.Controls.Dropdown.options = new List<Dropdown.OptionData>();

                    if (layer.CanBeEmpty) layer.Controls.Dropdown.options.Add(new Dropdown.OptionData("Empty", EmptyIcon));

                    layer.Controls.Dropdown.options.AddRange(layer.Content.Textures.Select(i => new Dropdown.OptionData(Regex.Replace(i.name, "([a-z])([A-Z])", "$1 $2"), Sprite.Create(layer.Content.GetIcon(i), new Rect(0, 0, 16, 16), Vector2.one / 2, 100))));
                    layer.Controls.Dropdown.value = -1;
                    layer.Controls.Dropdown.value = layer.Index + (layer.CanBeEmpty ? 1 : 0);
                    layer.Controls.Dropdown.onValueChanged.AddListener(value => SetIndex(layer, value));
                    layer.Controls.Prev.onClick.AddListener(() => Switch(layer, -1));
                    layer.Controls.Next.onClick.AddListener(() => Switch(layer, +1));
                    layer.Controls.Hide.onClick.AddListener(() => Hide(layer));
                    layer.Controls.Paint.onClick.AddListener(() => Paint(layer));
                    layer.Controls.Hue.onValueChanged.AddListener(value => Rebuild(layer));
                    layer.Controls.Saturation.onValueChanged.AddListener(value => Rebuild(layer));
                    layer.Controls.Brightness.onValueChanged.AddListener(value => Rebuild(layer));
                    layer.Controls.OnSelectFixedColor = color => { layer.Color = color; if (layer.Name == "Body") Layers.Single(i => i.Name == "Head").Color = color; Rebuild(layer); };
                }
            }

            Rebuild(null);
        }

        public void Rebuild()
        {
            Rebuild(null);
        }

        public void Hide(LayerEditor layer)
        {
            layer.Hidden = !layer.Hidden;
            Rebuild(layer);
        }

        public void Paint(LayerEditor layer)
        {
            ColorPicker.Open(layer.Color);
            ColorPicker.OnColorPicked = color =>
            {
                layer.Color = color;
                Rebuild(layer);
            };
        }

        private void Switch(LayerEditor layer, int direction)
        {
            layer.Switch(direction);
            Rebuild(layer);
        }

        private void SetIndex(LayerEditor layer, int index)
        {
            if (layer.CanBeEmpty) index--;

            layer.SetIndex(index);

            if (layer.Name == "Body")
            {
                Layers.Single(i => i.Name == "Head").SetIndex(index);
            }

            Rebuild(layer);
        }

        private void Rebuild(LayerEditor layer)
        {
            var layers = Layers.ToDictionary(i => i.Name, i => i.SpriteData);

            CharacterBuilder.Head = layers["Head"];
            CharacterBuilder.Body = layers["Body"];
            CharacterBuilder.Hair = layers["Hair"];
            CharacterBuilder.Armor = layers["Armor"];
            CharacterBuilder.Helmet = layers["Helmet"];
            CharacterBuilder.Weapon = layers["Weapon"];
            CharacterBuilder.Shield = layers["Shield"];
            CharacterBuilder.Cape = layers["Cape"];
            CharacterBuilder.Back = layers["Back"];
            CharacterBuilder.Rebuild(layer?.Name);
        }

        #if UNITY_EDITOR

        public void Save()
        {
            var path = EditorUtility.SaveFilePanel("Save as PNG", "", "SpriteSheet.png", "png");

            File.WriteAllBytes(path, CharacterBuilder.Texture.EncodeToPNG());
            Debug.Log($"Image saved as {path}.");
        }

        #endif
    }
}