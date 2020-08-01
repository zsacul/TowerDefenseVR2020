$fn = 12;
chosen = 3;
max_ = 4;
minkowski() {
union() {
cube([1,10*max_,25]);
if(chosen!= -1) {
translate([0, 10*chosen, -10]) {
cube([1, 10, 10]);
}
}
}
rotate([0, 90, 0]) {
cylinder(r=2,h=1);
}
}